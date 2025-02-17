DELETE FROM `landblock_instance` WHERE `landblock` = 0xA8AC;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7A8AC000,  2082, 0xA8AC0000, 36.0269, 16.3946, 84.005, 1, 0, 0, 0, False, '2005-02-09 10:00:00'); /* Hunter's Leap(2082/portalhuntersleap) */
/* @teleloc 0xA8AC0000 [36.026901 16.394600 84.004997] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7A8AC007,   218, 0xA8AC0102, 60.5426, 48.9019, 75.667, 0.822468, 0, 0, -0.568811,  True, '2005-02-09 10:00:00'); /* Black Rat(218/ratblack) - Level: 6 - T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Black Rat Tail(3681/rattailblack) */
/* @teleloc 0xA8AC0102 [60.542599 48.901901 75.667000] 0.822468 0.000000 0.000000 -0.568811 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7A8AC008,  1625, 0xA8AC0102, 60.1944, 55.8338, 75.667, 0.038854, 0, 0, 0.999245,  True, '2005-02-09 10:00:00'); /* Mountain Rat(1625/ratmountain) - Level: 16 - T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Hot Pepper(5794/hotpepper) */
/* @teleloc 0xA8AC0102 [60.194401 55.833801 75.667000] 0.038854 0.000000 0.000000 0.999245 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7A8AC009,   218, 0xA8AC0102, 61.5363, 54.3989, 75.667, 0.491545, 0, 0, 0.870852,  True, '2005-02-09 10:00:00'); /* Black Rat(218/ratblack) - Level: 6 - T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Black Rat Tail(3681/rattailblack) */
/* @teleloc 0xA8AC0102 [61.536301 54.398899 75.667000] 0.491545 0.000000 0.000000 0.870852 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7A8AC00A,  7923, 0xA8AC0102, 54.8578, 41.388, 75.66, 1, 0, 0, 0, False, '2005-02-09 10:00:00'); /* Linkable Monster Generator ( 3 Min.)(7923/linkmonstergen3minutes) - Generates - Place Holder Object(3666/placeholder) */
/* @teleloc 0xA8AC0102 [54.857800 41.388000 75.660004] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7A8AC00A, 0x7A8AC007, '2005-02-09 10:00:00')/* Black Rat (218/ratblack) - Level: 6 T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Black Rat Tail(3681/rattailblack) */
     , (0x7A8AC00A, 0x7A8AC008, '2005-02-09 10:00:00')/* Mountain Rat (1625/ratmountain) - Level: 16 T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Hot Pepper(5794/hotpepper) */
     , (0x7A8AC00A, 0x7A8AC009, '2005-02-09 10:00:00')/* Black Rat (218/ratblack) - Level: 6 T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Black Rat Tail(3681/rattailblack) */
     , (0x7A8AC00A, 0x7A8AC00B, '2005-02-09 10:00:00')/* Black Rat (218/ratblack) - Level: 6 T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Black Rat Tail(3681/rattailblack) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7A8AC00B,   218, 0xA8AC0102, 57.9949, 53.7781, 75.667, 0.899232, 0, 0, -0.437473,  True, '2005-02-09 10:00:00'); /* Black Rat(218/ratblack) - Level: 6 - T1_Warrior - Random Loot: Tier 1 - DeathTreasureType: 459(T1_Warrior) - Generates - Black Rat Tail(3681/rattailblack) */
/* @teleloc 0xA8AC0102 [57.994900 53.778099 75.667000] 0.899232 0.000000 0.000000 -0.437473 */
