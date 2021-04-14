using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PlayerInventory : Control
    {
        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("Inventory"))
            {
                Visible = !Visible;
            }
        }
    }
}
