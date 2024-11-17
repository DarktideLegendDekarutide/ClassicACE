using ACE.Entity;
using ACE.Server.Pathfinding.Geometry;
using ACE.Server.Entity;
using ACE.Server.Mods;
using ACE.Server.DotRecast.Core;
using ACE.Server.DotRecast.Core.Numerics;
using ACE.Server.DotRecast.Detour;
using ACE.Server.DotRecast.Detour.Io;
using ACE.Server.DotRecast.Recast;
using ACE.Server.DotRecast.Recast.Toolset;
using ACE.Server.DotRecast.Recast.Toolset.Tools;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Pathfinding
{
    public class PathFinder
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int VERTS_PER_POLY = 6;

        public string InsideMeshDirectory { get; private set; }
        public readonly ConcurrentDictionary<uint, DtNavMesh?> Meshes = new ConcurrentDictionary<uint, DtNavMesh?>();

        public PathFinder()
        {
            // Get the directory where the current executable is located
            var exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (exeLocation == null)
            {
                throw new InvalidOperationException("Unable to determine the executable location.");
            }

            // Set the path to 'Meshes/Inside' within the executable's directory
            InsideMeshDirectory = Path.Combine(exeLocation, "Meshes", "Inside");

            // Log or ensure the directory exists at runtime (this should be covered by the build step)
            if (!Directory.Exists(InsideMeshDirectory))
            {
                Directory.CreateDirectory(InsideMeshDirectory);
            }

            log.Info($"PathFinder initialized with InsideMeshDirectory at: {InsideMeshDirectory}");
        }

        /// <summary>
        /// Find a route to the end position.
        /// </summary>
        /// <param name="end">The ending position</param>
        /// <returns>A list of positions</returns>
        public List<Position>? FindRoute(Position start, Position end)
        {
            if (!TryGetMesh(start, out var mesh) || mesh is null)
            {
                return null;
            }

            if ((start.Cell & 0xFFFF0000) != (end.Cell & 0xFFFF0000))
            {
                log.Info($"FindRoute only works inside a single landblock.");
                return null;
            }

            var rc = new RcTestNavMeshTool();

            var halfExtents = new RcVec3f(1.25f, 1.25f, 1.25f);

            var query = new DtNavMeshQuery(mesh);
            var m_filter = new DtQueryDefaultFilter();

            var startStatus = query.FindNearestPoly(new RcVec3f(start.PositionX, start.PositionZ, start.PositionY), halfExtents, m_filter, out long startRef, out var startPt, out bool isStartOverPoly);
            var endStatus = query.FindNearestPoly(new RcVec3f(end.PositionX, end.PositionZ, end.PositionY), halfExtents, m_filter, out long endRef, out var endPt, out bool isEndOverPoly);

            var polys = new List<long>();
            var path = new List<DtStraightPath>();

            var res = rc.FindStraightPath(query, startRef, endRef, startPt, endPt, m_filter, true, ref polys, ref path, 0);

            //var res = rc.FindFollowPath(PluginCore.Instance.Nav?.Mesh, query, startRef, endRef, startPt, endPt, m_filter, false, ref polys, ref pts);

            // TODO: proper cell ids..
            return path.Select(p => new Position(start.Cell, new Vector3(p.pos.X, p.pos.Z, p.pos.Y), Quaternion.Identity)).ToList();
        }

        public Position? GetRandomPointOnMesh(Position start, float? maxDistance = null)
        {
            if (!TryGetMesh(start, out var mesh) || mesh is null)
            {
                return null;
            }

            var query = new DtNavMeshQuery(mesh);
            var m_filter = new DtQueryDefaultFilter();
            var frand = new RcRand(DateTime.Now.Ticks);
            var halfExtents = new RcVec3f(1.25f, 1.25f, 1.25f);

            var startStatus = query.FindNearestPoly(new RcVec3f(start.PositionX, start.PositionZ, start.PositionY), halfExtents, m_filter, out long startRef, out var startPt, out bool isStartOverPoly);

            if (startStatus.IsEmpty())
            {
                return null; 
            }

            if (maxDistance.HasValue)
            {
                query.FindRandomPointWithinCircle(startRef, startPt, maxDistance.Value, m_filter, frand,
                                                   out long randomRef, out RcVec3f randomPt);

                if (randomRef != 0)
                {
                    return new Position(start.Cell, new Vector3(randomPt.X, randomPt.Z, randomPt.Y), Quaternion.Identity);
                }
            }
            else
            {
                query.FindRandomPoint(m_filter, frand, out long randomRef, out RcVec3f randomPt);

                if (randomRef != 0)
                {
                    return new Position(start.Cell, new Vector3(randomPt.X, randomPt.Z, randomPt.Y), Quaternion.Identity);
                }
            }

            return null;
        }

        private bool TryGetMesh(Position pos, out DtNavMesh? mesh)
        {
            if (Meshes.TryGetValue(pos.Cell & 0xFFFF0000, out mesh))
            {
                return mesh is not null;
            }

            Meshes.TryAdd(pos.Cell & 0xFFFF0000, null);

            TryLoadMesh(pos);
            return false;
        }

        private void TryLoadMesh(Position pos)
        {
            _ = Task.Run(() =>
            {

                var geometry = new LandblockGeometry(pos.Cell & 0xFFFF0000);
                if (!geometry.DungeonCells.TryGetValue(pos.Cell, out var cellGeometry))
                {
                    log.Info($"Could not load cell geometry! {pos} cellGeometry:{cellGeometry}");
                    return;
                }

                Dictionary<uint, bool> checkedCells = new();
                var cells = geometry.DungeonCells.Values.ToList();

                var meshPath = Path.Combine(InsideMeshDirectory, $"{pos.Cell & 0xFFFF0000:X8}.mesh");
                if (File.Exists(meshPath))
                {
                    var meshReader = new DtMeshDataReader();

                    using (var stream = File.OpenRead(meshPath))
                    using (var reader = new BinaryReader(stream))
                    {
                        var rcBytes = new RcByteBuffer(reader.ReadBytes((int)stream.Length));
                        var meshData = meshReader.Read(rcBytes, VERTS_PER_POLY, true);

                        var mesh = new DtNavMesh(meshData, VERTS_PER_POLY, 0);
                        Meshes.TryUpdate(pos.Cell & 0xFFFF0000, mesh, null);
                        return;
                    }
                }

                var geom = CellGeometryProvider.LoadGeometry(geometry, cells);
                if (geom is null)
                {
                    log.Info($"Could not load cell geometry provider! {pos} cellGeometry:{geom} neighbors:{string.Join(",", cells.Select(n => $"{n.CellId:X8}"))}");
                    return;

                }

                var builder = new NavMeshBuilder();
                var settings = GetMeshSettings();
                var res = builder.Build(geom, settings);

                var meshWriter = new DtMeshDataWriter();
                using (var stream = File.OpenWrite(meshPath))
                using (var writer = new BinaryWriter(stream))
                {
                    meshWriter.Write(writer, res, RcByteOrder.LITTLE_ENDIAN, false);
                }

                var meshNew = new DtNavMesh(res, VERTS_PER_POLY, 0);
                Meshes.TryUpdate(pos.Cell & 0xFFFF0000, meshNew, null);
            });
        }

        private RcNavMeshBuildSettings GetMeshSettings()
        {
            return new RcNavMeshBuildSettings()
            {
                agentHeight = 2f, // made this a little extra, just to try and account for bigger mobs...
                agentMaxClimb = 0.95f,
                agentMaxSlope = 50f,
                cellHeight = 0.1f,
                cellSize = 0.1f,
                agentRadius = 0.45f,
                detailSampleDist = 6.0f,
                detailSampleMaxError = 1.0f,
                edgeMaxError = 1f,
                edgeMaxLen = 12.0f,
                mergedRegionSize = 20,
                minRegionSize = 8,
                vertsPerPoly = VERTS_PER_POLY,
                partitioning = (int)RcPartition.WATERSHED
            };
        }
    }
}

