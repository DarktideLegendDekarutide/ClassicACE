using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.DotRecast.Core {
    internal static class Extensions {
        public static void ArrayFill(Array a, object value) {
            ArrayFill(a, value, 0, a.Length);
        }
        public static void ArrayFill(Array a, object value, int startIndex, int count) {
            for (var i = startIndex; i < a.Length && i < startIndex + count; i++) {
                a.SetValue(value, i);
            }
        }

        public static bool FloatIsFinite(float v) {
            return !float.IsInfinity(v);
        }
        public static float MathClamp(float value, float min, float max) {
            return Math.Min(Math.Max(value, min), max);
        }
        public static int MathClamp(int value, int min, int max) {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
