DELETE FROM `weenie` WHERE `class_Id` = 3798;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (3798, 'javelinacid', 4, '2005-02-09 10:00:00') /* Missile */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (3798,   1,        256) /* ItemType - MissileWeapon */
     , (3798,   5,         10) /* EncumbranceVal */
     , (3798,   8,         10) /* Mass */
     , (3798,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (3798,  11,        250) /* MaxStackSize */
     , (3798,  12,          1) /* StackSize */
     , (3798,  13,         10) /* StackUnitEncumbrance */
     , (3798,  14,         10) /* StackUnitMass */
     , (3798,  15,         20) /* StackUnitValue */
     , (3798,  16,          1) /* ItemUseable - No */
     , (3798,  18,        256) /* UiEffects - Acid */
     , (3798,  19,         20) /* Value */
     , (3798,  44,          9) /* Damage */
     , (3798,  45,         32) /* DamageType - Acid */
     , (3798,  46,        128) /* DefaultCombatStyle - ThrownWeapon */
     , (3798,  48,         12) /* WeaponSkill - ThrownWeapon */
     , (3798,  49,         20) /* WeaponTime */
     , (3798,  51,          2) /* CombatUse - Missile */
     , (3798,  93,     132116) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, Inelastic */
     , (3798, 150,        103) /* HookPlacement - Hook */
     , (3798, 151,          2) /* HookType - Wall */
     , (3798, 169,  101188618) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (3798,  17, True ) /* Inelastic */
     , (3798,  69, False ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (3798,  22,    0.25) /* DamageVariance */
     , (3798,  27,       0) /* RotationSpeed */
     , (3798,  29,       1) /* WeaponDefense */
     , (3798,  62,       1) /* WeaponOffense */
     , (3798,  78,       1) /* Friction */
     , (3798,  79,       0) /* Elasticity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (3798,   1, 'Acid Javelin') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (3798,   1, 0x02000508) /* Setup */
     , (3798,   3, 0x20000014) /* SoundTable */
     , (3798,   8, 0x060010C9) /* Icon */
     , (3798,  22, 0x3400002B) /* PhysicsEffectTable */;
