using System;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class DragStartPayload
    {
        public ItemSlot originatingSlot;
        public ItemInventoryTile draggedTile;

        public DragStartPayload(ItemSlot originatingSlot, ItemInventoryTile draggedTile)
        {
            this.originatingSlot = originatingSlot;
            this.draggedTile = draggedTile;
        }
    }

    public class DragStopPayload
    {
        public ItemSlot destinationSlot;

        public DragStopPayload(ItemSlot destinationSlot)
        {
            this.destinationSlot = destinationSlot;
        }
    }

    public abstract class ItemSlot : Control
    {
        public static Action<ItemSlot> OnHoverStarted;
        public static Action<ItemSlot> OnHoverEnded;
        public static Action<DragStartPayload> OnDragStarted;
        public static Action<DragStopPayload> OnDragEnded;


        protected ItemInventoryTile currentTile = null;

        protected bool hovered = false;

        /// <summary>
        /// Can this Tile accept the drag and drop item?
        /// </summary>
        public virtual bool CanDropDnDItem(ItemInventoryTile tile)
        {
            return true;
        }


        /// <summary>
        /// Drop the item tile onto this Slot
        /// </summary>
        public virtual void DropDnDItem(ItemInventoryTile tile)
        {
            AddItemTile(tile);
            tile.Visible = true;
        }

        /// <summary>
        /// Cancel the drag and drop operation.
        /// </summary>
        public virtual void CancelDnD()
        {
            if (currentTile != null)
            {
                currentTile.Visible = true;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (!IsVisibleInTree()) return;

            if (hovered && @event is InputEventMouseButton mb)
            {
                HandleDragging(mb);
            }
            else if (@event is InputEventMouseMotion mm)
            {
                HandleHovering();
            }
        }

        private void HandleHovering()
        {
            if (hovered)
            {
                Vector2 mpos = GetGlobalMousePosition();
                bool hasPoint = GetGlobalRect().HasPoint(mpos);

                if (!hasPoint)
                {
                    hovered = false;
                    OnHoverEnded?.Invoke(this);
                }
            }
            else
            {
                Vector2 mpos = GetGlobalMousePosition();
                bool hasPoint = GetGlobalRect().HasPoint(mpos);
                if (hasPoint)
                {
                    hovered = true;
                    OnHoverStarted?.Invoke(this);
                }
            }
        }

        private void HandleDragging(InputEventMouseButton mb)
        {
            if (mb.IsActionPressed("LeftClick") && currentTile != null)
            {
                DragStartPayload payload = new DragStartPayload(this, currentTile);
                OnDragStarted?.Invoke(payload);
                currentTile.Visible = false;
            }
            else if (mb.IsActionReleased("LeftClick"))
            {
                DragStopPayload payload = new DragStopPayload(this);
                OnDragEnded?.Invoke(payload);
            }
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
        public ItemInventoryTile GetItemTile() => currentTile;
    }
}