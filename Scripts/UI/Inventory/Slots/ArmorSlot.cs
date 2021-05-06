using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Slots
{
    public class ArmorSlot : EquipmentSlot
    {
        [Export] private ArmorSlotType armorSlotType = ArmorSlotType.HEAD;

        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            GD.Print("ArmorSlot - CanDropDnDItem");
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
        
        protected override EquipPayload GetPayload()
        {
            GD.Print("ArmorSlot - GetPayload");
            IEquipable equipable = currentTile.GetParentItem() as IEquipable;
            if (equipable != null)
            {
                return new ArmorEquipPayload(
                    equipable,
                    armorSlotType
                );
            }

            return null;
        }
    }
}