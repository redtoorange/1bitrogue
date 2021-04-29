using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class TrinketSlot : ItemSlot
    {
        [Export] private Items.TrinketSlot trinketSlotType;

        protected override void HoverStarted()
        {
            GD.Print($"HoverStarted {trinketSlotType}");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded {trinketSlotType}");
        }
    }
}