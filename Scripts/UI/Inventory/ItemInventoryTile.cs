using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class ItemInventoryTile : TextureRect
    {
        private bool hovered = false;
        private bool dragging = false;
    
    
    
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

        private Vector2 offset = Vector2.Zero;
        private Vector2 startPosition = Vector2.Zero;
    
        private void StartDragging()
        {
            dragging = true;

            startPosition = RectPosition;
            offset = GetGlobalMousePosition() - RectPosition;
        }

        private void EndDragging()
        {
            dragging = false;
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
    }
}