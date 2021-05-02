using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public abstract class ItemSlot : Control
    {
        protected ItemInventoryTile currentTile = null;


        public override void _Ready()
        {
            Connect("mouse_entered", this, nameof(HoverStarted));
            Connect("mouse_exited", this, nameof(HoverEnded));
        }

        protected virtual void HoverStarted()
        {
            GD.Print("HoverStarted: ItemSlot");
        }

        protected virtual void HoverEnded()
        {
            GD.Print("HoverEnded: ItemSlot");
        }

        public virtual void AddItemTile(ItemInventoryTile tile)
        {
            tile.SetSlot(this);
            currentTile = tile;

            AddChild(tile);
        }

        public virtual void RemoveItemTile(ItemInventoryTile tile)
        {
            RemoveChild(tile);
            currentTile = null;
        }

        public bool IsOccupied() => currentTile != null;

        public override bool CanDropData(Vector2 position, object data)
        {
            return !IsOccupied();
        }

        public override void DropData(Vector2 position, object data)
        {
            if (data is DragAndDropPayload payload)
            {
                ItemInventoryTile tile = payload.draggedTile;
                payload.originalSlot.RemoveItemTile(tile);
                AddItemTile(tile);
            }
        }

        public override object GetDragData(Vector2 position)
        {
            if (currentTile == null) return null;

            TextureRect image = new TextureRect();
            image.Texture = currentTile.Texture;
            image.RectPosition = new Vector2(-8, -8);

            Control preview = new Control();
            preview.AddChild(image);
            SetDragPreview(preview);

            return new DragAndDropPayload(this, currentTile);
        }
    }

    public class DragAndDropPayload : Object
    {
        public ItemSlot originalSlot;
        public ItemInventoryTile draggedTile;

        public DragAndDropPayload(ItemSlot originalSlot, ItemInventoryTile draggedTile)
        {
            this.originalSlot = originalSlot;
            this.draggedTile = draggedTile;
        }
    }
}