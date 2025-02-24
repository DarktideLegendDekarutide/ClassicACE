DELETE FROM `weenie` WHERE `class_Id` = 25841;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (25841, 'leggingsscourge', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (25841,   1,          2) /* ItemType - Armor */
     , (25841,   3,         39) /* PaletteTemplate - Black */
     , (25841,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (25841,   5,        600) /* EncumbranceVal */
     , (25841,   8,        360) /* Mass */
     , (25841,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (25841,  16,          1) /* ItemUseable - No */
     , (25841,  19,       8750) /* Value */
     , (25841,  33,          1) /* Bonded - Bonded */
     , (25841,  27,          4) /* ArmorType - StuddedLeather */
     , (25841,  28,        300) /* ArmorLevel */
     , (25841,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (25841, 106,        400) /* ItemSpellcraft */
     , (25841, 107,       1000) /* ItemCurMana */
     , (25841, 108,       1000) /* ItemMaxMana */
     , (25841, 109,        125) /* ItemDifficulty */
     , (25841, 158,          7) /* WieldRequirements - Level */
     , (25841, 159,          1) /* WieldSkillType - Axe */
     , (25841, 160,        100) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (25841,  22, True ) /* Inscribable */
     , (25841, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (25841,   5,  -0.033) /* ManaRate */
     , (25841,  12,    0.66) /* Shade */
     , (25841,  13,     0.7) /* ArmorModVsSlash */
     , (25841,  14,     0.7) /* ArmorModVsPierce */
     , (25841,  15,     0.9) /* ArmorModVsBludgeon */
     , (25841,  16,     0.2) /* ArmorModVsCold */
     , (25841,  17,     0.7) /* ArmorModVsFire */
     , (25841,  18,     0.9) /* ArmorModVsAcid */
     , (25841,  19,     0.2) /* ArmorModVsElectric */
     , (25841, 110,       1) /* BulkMod */
     , (25841, 111,       1) /* SizeMod */
     , (25841, 10003,      -3) /* MeleeDefenseCap */
     , (25841, 10004,      -3) /* MissileDefenseCap */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (25841,   1, 'Scourge''s Hide Leggings') /* Name */
     , (25841,  15, 'These leggings were crafted from the hide of the plague ridden hide of the dreaded rat, Scourge.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (25841,   1, 0x020001A8) /* Setup */
     , (25841,   3, 0x20000014) /* SoundTable */
     , (25841,   6, 0x0400007E) /* PaletteBase */
     , (25841,   7, 0x10000512) /* ClothingBase */
     , (25841,   8, 0x0600301A) /* Icon */
     , (25841,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (25841,  2616,      2)  /* Minor Acid Ward */
     , (25841,  2617,      2)  /* Minor Bludgeoning Ward */;
