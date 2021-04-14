using GameboyRoguelike.Scripts.Characters.Player;
using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.Items
{
    public class Item : Node2D
    {
        [Export(PropertyHint.ResourceType)] protected ItemStats stats;
        
        private Sprite itemSprite;
        private Area2D area2D;

        public override void _Ready()
        {
            itemSprite = GetNode<Sprite>("Sprite");
            itemSprite.Texture = stats.worldSprite;

            area2D = GetNode<Area2D>("Area2D");
            area2D.Connect("body_entered", this, nameof(HandleBodyEntered));
            area2D.Connect("body_exited", this, nameof(HandleBodyExited));
        }

        private void HandleBodyEntered(PhysicsBody2D other)
        {
            if (other is Player p)
            {
                p.GetGroundItemController().AddItem(this);
            }
        }

        private void HandleBodyExited(PhysicsBody2D other)
        {
            if (other is Player p)
            {
                p.GetGroundItemController().RemoveItem(this);
            }
        }

        public void SetCollisionEnabled(bool enabled)
        {
            area2D.Monitoring = enabled;
        }

        public void SetSpriteEnabled(bool enabled)
        {
            itemSprite.Visible = enabled;
        }
    }
}