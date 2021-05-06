using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Slots
{
    public class ArmorSlot : ItemSlot
    {
        [Export] private ArmorSlotType armorSlotType = ArmorSlotType.HEAD;

        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            Item item = tile.GetParentItem();
            // Cast to Armor and get stats
            if (item is IEquipable && item is Armor a)
            {
                ArmorStats stats = a.GetStats();
                // Verify matching slot types
                if (stats.slotType == armorSlotType)
                {
                    // Return true even if we are occupied so we can swap the items
                    return true;
                }
            }

            // Failed the conversion tree, so the item is not the right type for this slot
            return false;
        }

        public override void AddItemTile(ItemInventoryTile tile)
        {
            base.AddItemTile(tile);
        }

        public override void RemoveItemTile()
        {
            base.RemoveItemTile();
        }
    }
}