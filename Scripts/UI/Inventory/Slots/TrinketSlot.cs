using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Slots
{
    public class TrinketSlot : EquipmentSlot
    {
        [Export] private TrinketSlotType trinketSlotType = TrinketSlotType.LEFT_RING;

        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            GD.Print("TrinketSlot - CanDropDnDItem");
            Item item = tile.GetParentItem();
            // Cast to Armor and get stats
            if (item is IEquipable && item is Trinket t)
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

            // Failed the conversion tree, so the item is not the right type for this slot
            return false;
        }

        protected override EquipPayload GetPayload()
        {
            GD.Print("TrinketSlot - GetPayload");
            IEquipable equipable = currentTile.GetParentItem() as IEquipable;
            if (equipable != null)
            {
                return new TrinketEquipPayload(
                    equipable,
                    trinketSlotType
                );
            }

            return null;
        }
    }
}