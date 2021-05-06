using Godot;

namespace BitRoguelike.Scripts.Map.Objects
{
    public class Bookcase : Node2D, IInteractable
    {
        public void Interact()
        {
            GD.Print("Interacting with bookcase");
        }
    }
}