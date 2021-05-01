using GameboyRoguelike.Scripts.Characters.Player;
using GameboyRoguelike.Scripts.Items.Stats;
using GameboyRoguelike.Scripts.Managers;
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

        private void SetCollisionEnabled(bool enabled)
        {
            area2D.Monitoring = enabled;
        }

        private void SetSpriteEnabled(bool enabled)
        {
            itemSprite.Visible = enabled;
        }

        public void OnPickedUp()
        {
            SetSpriteEnabled(false);
            SetCollisionEnabled(false);
                
            ItemManager.S.RemoveChild(this);
        }

        public void OnDropped()
        {
            ItemManager.S.AddChild(this);
                
            SetSpriteEnabled(true);
            SetCollisionEnabled(false);
        }
    }
}