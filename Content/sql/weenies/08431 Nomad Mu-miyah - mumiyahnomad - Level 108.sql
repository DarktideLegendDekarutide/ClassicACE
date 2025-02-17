DELETE FROM `weenie` WHERE `class_Id` = 8431;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (8431, 'mumiyahnomad', 10, '2019-09-13 00:00:00') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (8431,   1,         16) /* ItemType - Creature */
     , (8431,   2,         14) /* CreatureType - Undead */
     , (8431,   3,         43) /* PaletteTemplate - LightBrown */
     , (8431,   6,         -1) /* ItemsCapacity */
     , (8431,   7,         -1) /* ContainersCapacity */
     , (8431,  16,          1) /* ItemUseable - No */
     , (8431,  25,        108) /* Level */
     , (8431,  27,          0) /* ArmorType - None */
     , (8431,  40,          1) /* CombatMode - NonCombat */
     , (8431,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (8431,  72,         14) /* FriendType - Undead */
     , (8431,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (8431, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (8431, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (8431, 140,          1) /* AiOptions - CanOpenDoors */
     , (8431, 146,      31416) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (8431,   1, True ) /* Stuck */
     , (8431,   6, True ) /* AiUsesMana */
     , (8431,  11, False) /* IgnoreCollisions */
     , (8431,  12, True ) /* ReportCollisions */
     , (8431,  13, False) /* Ethereal */
     , (8431,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (8431,   1,       5) /* HeartbeatInterval */
     , (8431,   2,       0) /* HeartbeatTimestamp */
     , (8431,   3,    0.65) /* HealthRate */
     , (8431,   4,     0.5) /* StaminaRate */
     , (8431,   5,       2) /* ManaRate */
     , (8431,  12,       1) /* Shade */
     , (8431,  13,    0.66) /* ArmorModVsSlash */
     , (8431,  14,    0.56) /* ArmorModVsPierce */
     , (8431,  15,    0.66) /* ArmorModVsBludgeon */
     , (8431,  16,    0.24) /* ArmorModVsCold */
     , (8431,  17,     0.4) /* ArmorModVsFire */
     , (8431,  18,    0.66) /* ArmorModVsAcid */
     , (8431,  19,    0.46) /* ArmorModVsElectric */
     , (8431,  31,      24) /* VisualAwarenessRange */
     , (8431,  34,     0.9) /* PowerupTime */
     , (8431,  36,       1) /* ChargeSpeed */
     , (8431,  39,     1.3) /* DefaultScale */
     , (8431,  64,    0.75) /* ResistSlash */
     , (8431,  65,    0.58) /* ResistPierce */
     , (8431,  66,    0.75) /* ResistBludgeon */
     , (8431,  67,       1) /* ResistFire */
     , (8431,  68,    0.25) /* ResistCold */
     , (8431,  69,    0.75) /* ResistAcid */
     , (8431,  70,    0.46) /* ResistElectric */
     , (8431,  71,       1) /* ResistHealthBoost */
     , (8431,  72,       1) /* ResistStaminaDrain */
     , (8431,  73,       1) /* ResistStaminaBoost */
     , (8431,  74,       1) /* ResistManaDrain */
     , (8431,  75,       1) /* ResistManaBoost */
     , (8431,  80,       3) /* AiUseMagicDelay */
     , (8431, 104,      10) /* ObviousRadarRange */
     , (8431, 122,       2) /* AiAcquireHealth */
     , (8431, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (8431,   1, 'Nomad Mu-miyah') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (8431,   1, 0x02000001) /* Setup */
     , (8431,   2, 0x09000025) /* MotionTable */
     , (8431,   3, 0x2000001E) /* SoundTable */
     , (8431,   4, 0x30000000) /* CombatTable */
     , (8431,   6, 0x0400007E) /* PaletteBase */
     , (8431,   7, 0x100000BD) /* ClothingBase */
     , (8431,   8, 0x060016C2) /* Icon */
     , (8431,  22, 0x34000028) /* PhysicsEffectTable */
     , (8431,  32,        335) /* WieldedTreasureType - 
                                   # Set: 1
                                   |  20.00% chance of Acid Yari (23722)
                                   |  25.00% chance of Yari (23730)
                                   |  10.00% chance of Acid Spear (23688)
                                   |  10.00% chance of Spear (23696)
                                   |  10.00% chance of Fire Tachi (23707)
                                   |  10.00% chance of Tachi (23700)
                                   |  15.00% chance of nothing from this set
                                   # Set: 2
                                   |  55.00% chance of Kite Shield (23684)
                                   |  45.00% chance of nothing from this set */
     , (8431,  35,        460) /* DeathTreasureType - T4_Magic - Loot Tier: 4 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (8431,   1, 240, 0, 0) /* Strength */
     , (8431,   2, 240, 0, 0) /* Endurance */
     , (8431,   3, 130, 0, 0) /* Quickness */
     , (8431,   4, 180, 0, 0) /* Coordination */
     , (8431,   5, 230, 0, 0) /* Focus */
     , (8431,   6, 240, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (8431,   1,   200, 0, 0, 320) /* MaxHealth */
     , (8431,   3,   200, 0, 0, 440) /* MaxStamina */
     , (8431,   5,   200, 0, 0, 440) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (8431,  1, 0, 3, 0, 290, 0, 0) /* Axe                 Specialized */
     , (8431,  6, 0, 3, 0, 355, 0, 0) /* MeleeDefense        Specialized */
     , (8431,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (8431, 12, 0, 3, 0, 120, 0, 0) /* ThrownWeapon        Specialized */
     , (8431, 14, 0, 3, 0, 300, 0, 0) /* ArcaneLore          Specialized */
     , (8431, 15, 0, 3, 0, 230, 0, 0) /* MagicDefense        Specialized */
     , (8431, 20, 0, 3, 0,  90, 0, 0) /* Deception           Specialized */
     , (8431, 31, 0, 3, 0, 145, 0, 0) /* CreatureEnchantment Specialized */
     , (8431, 33, 0, 3, 0, 145, 0, 0) /* LifeMagic           Specialized */
     , (8431, 34, 0, 3, 0, 145, 0, 0) /* WarMagic            Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (8431,  0,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (8431,  1,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (8431,  2,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (8431,  3,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (8431,  4,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (8431,  5,  4, 65, 0.75,  240,  158,  134,  158,   58,   96,  158,  110,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (8431,  6,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (8431,  7,  4,  0,    0,  240,  158,  134,  158,   58,   96,  158,  110,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (8431,  8,  4, 70, 0.75,  240,  158,  134,  158,   58,   96,  158,  110,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (8431,    62,  2.007)  /* Acid Stream V */
     , (8431,    68,  2.007)  /* Shock Wave V */
     , (8431,    73,  2.007)  /* Frost Bolt V */
     , (8431,    79,   2.01)  /* Lightning Bolt V */
     , (8431,    84,  2.007)  /* Flame Bolt V */
     , (8431,    90,  2.007)  /* Force Bolt V */
     , (8431,    96,  2.007)  /* Whirling Blade V */
     , (8431,   129,   2.01)  /* Acid Volley V */
     , (8431,   137,   2.01)  /* Frost Volley V */
     , (8431,   141,   2.01)  /* Lightning Volley V */
     , (8431,   145,   2.01)  /* Flame Volley V */
     , (8431,   169,  2.025)  /* Regeneration Self V */
     , (8431,   175,   2.02)  /* Fester Other V */
     , (8431,   198,   2.02)  /* Exhaustion Other V */
     , (8431,  1175,   2.02)  /* Harm Other V */
     , (8431,  1199,   2.02)  /* Enfeeble Other V */
     , (8431,  1223,   2.02)  /* Mana Drain Other V */
     , (8431,  1241,  2.025)  /* Drain Health Other V */
     , (8431,  1253,  2.025)  /* Drain Stamina Other V */
     , (8431,  1264,  2.025)  /* Drain Mana Other V */;

INSERT INTO `weenie_properties_event_filter` (`object_Id`, `event`)
VALUES (8431,  94) /* ATTACK_NOTIFICATION_EVENT */
     , (8431, 414) /* PLAYER_DEATH_EVENT */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (8431,  5 /* HeartBeat */,  0.015, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000084 /* Point */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (8431,  5 /* HeartBeat */,   0.04, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x1300007A /* Beckon */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (8431,  5 /* HeartBeat */,  0.055, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000096 /* Slouch */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (8431,  5 /* HeartBeat */,   0.15, NULL, 0x8000003E /* SwordCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (8431, 9,  6876,  0, 0, 0.02, False) /* Create Sturdy Iron Key (6876) for ContainTreasure */
     , (8431, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
