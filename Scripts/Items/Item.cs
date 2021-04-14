using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.Items
{
    public class Item : Node2D
    {
        [Export(PropertyHint.ResourceType)] protected ItemStats stats;
        private Sprite itemSprite;

        public override void _Ready()
        {
            itemSprite = GetNode<Sprite>("Sprite");
            itemSprite.Texture = stats.worldSprite;

            GetNode<Area2D>("Area2D").Connect("body_entered", this, nameof(HandleBodyEntered));
            GetNode<Area2D>("Area2D").Connect("body_exited", this, nameof(HandleBodyExited));
        }

        private void HandleBodyEntered(PhysicsBody2D other)
        {
            GD.Print(other.Name + " Entered " + Name);
        }
        
        private void HandleBodyExited(PhysicsBody2D other)
        {
            GD.Print(other.Name + " Exited " + Name);
        }
    }
}