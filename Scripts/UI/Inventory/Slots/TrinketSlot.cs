using GameboyRoguelike.Scripts.Items;
using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class TrinketSlot : ItemSlot
    {
        [Export] private TrinketSlotType trinketSlotType;

        protected override void HoverStarted()
        {
            GD.Print($"HoverStarted {trinketSlotType}");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded {trinketSlotType}");
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
                if (item is Equipable && item is Trinket t)
                {
                    TrinketStats stats = t.GetStats();
                    // Verify matching slot types
                    if (stats.equipType == TrinketEquipType.RING &&
                        (trinketSlotType == TrinketSlotType.LEFT_RING || trinketSlotType == TrinketSlotType.RIGHT_RING))
                    {
                        // Return true even if we are occupied so we can swap the items
                        return true;
                    }
                }
            }

            // Failed the conversion tree, so the item is not the right type for this slot
            return false;
        }
    }
}