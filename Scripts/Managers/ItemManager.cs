using Godot;

namespace GameboyRoguelike.Scripts.Managers
{
    public class ItemManager : Node2D
    {
        public static ItemManager S;

        public override void _Ready()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                GD.PrintErr("More than one Item Manager in World");
                QueueFree();
            }
        }

        public override void _ExitTree()
        {
            S = null;
        }
    }
}