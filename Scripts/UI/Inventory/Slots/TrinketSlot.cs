using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class TrinketSlot : ItemSlot
    {
        [Export] private Items.TrinketSlotType trinketSlotType;

        protected override void HoverStarted()
        {
            GD.Print($"HoverStarted {trinketSlotType}");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded {trinketSlotType}");
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