using BitRoguelike.Scripts.Characters.Player;
using BitRoguelike.Scripts.Managers;
using Godot;

namespace BitRoguelike.Scripts.Items
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

        public Texture GetWorldTexture() => stats.worldSprite;
        public Texture GetUiTexture() => stats.inventoryIcon;

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

        public void SetEnabled(bool enabled)
        {
            area2D.Monitoring = enabled;
            itemSprite.Visible = enabled;
            Visible = enabled;
        }

        public void OnPickedUp()
        {
            SetEnabled(false);
            ItemManager.S.RemoveChild(this);
        }

        public void OnDropped(Vector2 dropLocation)
        {
            ItemManager.S.AddChild(this);
            GlobalPosition = dropLocation;
            SetEnabled(true);
        }
    }
}