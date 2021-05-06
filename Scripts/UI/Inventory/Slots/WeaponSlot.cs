using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Slots
{
    public class WeaponSlot : EquipmentSlot
    {
        [Export] private WeaponSlotType weaponSlotTypeType = WeaponSlotType.MAIN_HAND;

        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            GD.Print("WeaponSlot - CanDropDnDItem");
            Item item = tile.GetParentItem();

            // Cast to Armor and get stats
            if (item is IEquipable && item is Weapon w)
            {
                WeaponStats stats = w.GetStats();
                // Verify matching slot types
                if (weaponSlotTypeType == WeaponSlotType.MAIN_HAND)
                {
                    return stats.weaponEquipType != WeaponEquipType.OFF_HAND;
                }
                else if (weaponSlotTypeType == WeaponSlotType.OFF_HAND)
                {
                    return stats.weaponEquipType != WeaponEquipType.MAIN_HAND;
                }
            }

            // Failed the conversion tree, so the item is not the right type for this slot
            return false;
        }

        protected override EquipPayload GetPayload()
        {
            GD.Print("WeaponSlot - GetPayload");
            IEquipable equipable = currentTile.GetParentItem() as IEquipable;
            if (equipable != null)
            {
                return new WeaponEquipPayload(
                    equipable,
                    weaponSlotTypeType
                );
            }

            return null;
        }
    }
}