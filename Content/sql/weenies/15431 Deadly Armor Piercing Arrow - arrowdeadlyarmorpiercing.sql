DELETE FROM `weenie` WHERE `class_Id` = 15431;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (15431, 'arrowdeadlyarmorpiercing', 5, '2019-12-25 00:00:00') /* Ammunition */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (15431,   1,        256) /* ItemType - MissileWeapon */
     , (15431,   3,         61) /* PaletteTemplate - White */
     , (15431,   5,         10) /* EncumbranceVal */
     , (15431,   8,          2) /* Mass */
     , (15431,   9,    8388608) /* ValidLocations - MissileAmmo */
     , (15431,  11,        250) /* MaxStackSize */
     , (15431,  12,          1) /* StackSize */
     , (15431,  13,         10) /* StackUnitEncumbrance */
     , (15431,  14,          2) /* StackUnitMass */
     , (15431,  15,          9) /* StackUnitValue */
     , (15431,  16,          1) /* ItemUseable - No */
     , (15431,  19,          9) /* Value */
     , (15431,  44,         30) /* Damage */
     , (15431,  45,          2) /* DamageType - Pierce */
     , (15431,  50,          1) /* AmmoType - Arrow */
     , (15431,  51,          3) /* CombatUse - Ammo */
     , (15431,  93,     132116) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, Inelastic */
     , (15431, 150,        103) /* HookPlacement - Hook */
     , (15431, 151,          2) /* HookType - Wall */
     , (15431, 158,          2) /* WieldRequirements - RawSkill */
     , (15431, 159,          2) /* WieldSkillType - Bow */
     , (15431, 160,        235) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (15431,  17, True ) /* Inelastic */
     , (15431,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (15431,  22,     0.2) /* DamageVariance */
     , (15431,  29,       1) /* WeaponDefense */
     , (15431,  39,     1.1) /* DefaultScale */
     , (15431,  62,       1) /* WeaponOffense */
     , (15431,  78,       1) /* Friction */
     , (15431,  79,       0) /* Elasticity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (15431,   1, 'Deadly Armor Piercing Arrow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (15431,   1, 0x02000124) /* Setup */
     , (15431,   3, 0x20000014) /* SoundTable */
     , (15431,   6, 0x04000BEF) /* PaletteBase */
     , (15431,   7, 0x1000034F) /* ClothingBase */
     , (15431,   8, 0x06002493) /* Icon */
     , (15431,  22, 0x3400002B) /* PhysicsEffectTable */;
