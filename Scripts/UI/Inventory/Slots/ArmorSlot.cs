using GameboyRoguelike.Scripts.Items;
using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class ArmorSlot : ItemSlot
    {
        [Export] private ArmorSlotType armorSlotType = ArmorSlotType.HEAD;

        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            Item item = tile.GetParentItem();
            // Cast to Armor and get stats
            if (item is Equipable && item is Armor a)
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
    }
}