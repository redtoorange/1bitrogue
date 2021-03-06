using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory
{
    public class ItemInventoryTile : TextureRect
    {
        private ItemSlot previousSlot = null;
        private bool hovered = false;
        private bool dragging = false;
        private Vector2 offset = Vector2.Zero;
        private Vector2 startPosition = Vector2.Zero;
        private Item parentItem = null;
        public void SetSlot(ItemSlot slot)
        {
            previousSlot = slot;
        }

        public void Init(Item parentItem)
        {
            this.parentItem = parentItem;
            
            Texture = this.parentItem.GetUiTexture();
        }

        public override void _Process(float delta)
        {
            if (!Visible) return;

            if (hovered && !dragging)
            {
                if (Input.IsActionPressed("LeftClick"))
                {
                    StartDragging();
                }
            }
            else if (dragging)
            {
                if (!Input.IsActionPressed("LeftClick"))
                {
                    EndDragging();
                }
                else
                {
                    Drag();
                }
            }
        }

        private void StartDragging()
        {
            // GD.Print("Started Dragging");
            dragging = true;

            MouseFilter = MouseFilterEnum.Ignore;                

            startPosition = RectPosition;
            offset = GetGlobalMousePosition() - RectPosition;
        }

        private void EndDragging()
        {
            // GD.Print("Stopped Dragging");
            dragging = false;
            
            MouseFilter = MouseFilterEnum.Stop;      
            
            RectPosition = startPosition;
        }

        private void Drag()
        {
            RectPosition = GetGlobalMousePosition() - offset;
        }

        public override void _Ready()
        {
            Connect("mouse_entered", this, nameof(HoverStarted));
            Connect("mouse_exited", this, nameof(HoverEnded));
        }

        protected virtual void HoverStarted()
        {
            hovered = true;
        }

        protected virtual void HoverEnded()
        {
            hovered = false;
        }

        public Item GetParentItem()
        {
            return parentItem;
        }
    }
}