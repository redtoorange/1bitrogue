using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class ArmorSlot : ItemSlot
    {
        [Export] private Items.ArmorSlotType armorSlotType = Items.ArmorSlotType.HEAD;
        
        protected override void HoverStarted(){
            GD.Print($"HoverStarted {armorSlotType}");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded {armorSlotType}");
        }
        
        public override void AddItemTile(ItemInventoryTile tile)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveItemTile(ItemInventoryTile tile)
        {
            throw new System.NotImplementedException();
        }
    }
}