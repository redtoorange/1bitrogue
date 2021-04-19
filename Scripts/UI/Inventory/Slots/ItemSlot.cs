using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class ItemSlot : Control
    {
        public override void _Ready()
        {
            Connect("mouse_entered", this, nameof(HoverStarted));
            Connect("mouse_exited", this, nameof(HoverEnded));
        }

        protected virtual void HoverStarted()
        {
            GD.Print("HoverStarted: ItemSlot");
        }

        protected virtual void HoverEnded()
        {
            GD.Print("HoverEnded: ItemSlot");
        }

        protected virtual void PlaceItemInSlot()
        {
            
        }
    }
}