using GameboyRoguelike.Scripts.Items;
using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class WeaponSlot : ItemSlot
    {
        [Export] private Items.WeaponSlot weaponSlotType;

        protected override void HoverStarted()
        {
            GD.Print($"HoverStarted {weaponSlotType}");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded {weaponSlotType}");
        }

        public override bool CanDropData(Vector2 position, object data)
        {
            // Convert to payload
            if (data is DragAndDropPayload payload)
            {
                // Get the item from the Tile
                ItemInventoryTile tile = payload.draggedTile;
                Item item = tile.GetParentItem();

                // Cast to Armor and get stats
                if (item is Equipable && item is Weapon w)
                {
                    WeaponStats stats = w.GetStats();
                    // Verify matching slot types
                    if (weaponSlotType == Items.WeaponSlot.MAIN_HAND)
                    {
                        return stats.weaponEquipType != WeaponEquipType.OFF_HAND;
                    }
                    else if (weaponSlotType == Items.WeaponSlot.OFF_HAND)
                    {
                        return stats.weaponEquipType != WeaponEquipType.MAIN_HAND;;
                    }
                }
            }

            // Failed the conversion tree, so the item is not the right type for this slot
            return false;
        }
    }
}