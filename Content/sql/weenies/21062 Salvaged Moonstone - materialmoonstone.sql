DELETE FROM `weenie` WHERE `class_Id` = 21062;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (21062, 'materialmoonstone', 44, '2005-02-09 10:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (21062,   1, 1073741824) /* ItemType - TinkeringMaterial */
     , (21062,   3,          8) /* PaletteTemplate - Green */
     , (21062,   5,         10) /* EncumbranceVal */
     , (21062,   8,        100) /* Mass */
     , (21062,   9,          0) /* ValidLocations - None */
     , (21062,  11,          1) /* MaxStackSize */
     , (21062,  12,          1) /* StackSize */
     , (21062,  13,         10) /* StackUnitEncumbrance */
     , (21062,  14,        100) /* StackUnitMass */
     , (21062,  15,         10) /* StackUnitValue */
     , (21062,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (21062,  19,         10) /* Value */
     , (21062,  33,          1) /* Bonded - Bonded */
     , (21062,  91,        100) /* MaxStructure */
     , (21062,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (21062,  94, 1073777039) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget, TinkeringMaterial */
     , (21062, 131,         31) /* MaterialType - Moonstone */
     , (21062, 150,        103) /* HookPlacement - Hook */
     , (21062, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (21062,  22, True ) /* Inscribable */
     , (21062,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (21062,   1, 'Salvaged Moonstone') /* Name */
     , (21062,  14, 'Apply this material to a treasure-generated item to increase its maximum mana by 1500.') /* Use */
     , (21062,  15, 'Chips of moonstone material salvaged from old items.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (21062,   1, 0x02000181) /* Setup */
     , (21062,   3, 0x20000014) /* SoundTable */
     , (21062,   6, 0x04000BEF) /* PaletteBase */
     , (21062,   7, 0x100003CE) /* ClothingBase */
     , (21062,   8, 0x0600102C) /* Icon */
     , (21062,  22, 0x3400002B) /* PhysicsEffectTable */
     , (21062,  50, 0x06002705) /* IconOverlay */;
