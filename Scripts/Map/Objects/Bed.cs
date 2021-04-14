using Godot;

namespace GameboyRoguelike.Scripts.Map.Objects
{
    public class Bed : Node2D, IInteractable
    {
        public void Interact()
        {
            GD.Print("Interacting with bed");
        }
    }
}