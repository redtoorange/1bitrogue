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

        public Action<ItemInventoryTile> OnDropItemOnGround;
        public Action<ItemSlot> OnShowContextMenu;

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

            if (@event is InputEventMouseMotion mm)
            {
                HandleHovering();
            }

            if (hovered && @event is InputEventMouseButton mb)
            {
                HandleClicks(mb);
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

        private void HandleClicks(InputEventMouseButton mb)
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

            if (mb.IsActionPressed("RightClick"))
            {
                GetTree().SetInputAsHandled();
                OnShowContextMenu?.Invoke(this);
            }
        }

        /// <summary>
        /// Add an item to a tile.  This should be used as a hook for equipment to equip
        /// </summary>
        public virtual void AddItemTile(ItemInventoryTile tile)
        {
            tile.SetSlot(this);
            currentTile = tile;

            AddChild(tile);
        }

        /// <summary>
        /// Remove an item from a tile.  This should be used as a hook for equipment to unequip
        /// </summary>
        public virtual void RemoveItemTile()
        {
            RemoveChild(currentTile);
            currentTile = null;
        }

        /// <summary>
        /// Just process and forward the event.  Equipment tiles should override this to handle unequipping
        /// </summary>
        public virtual void ContextMenuNotifyDrop()
        {
            ItemInventoryTile tempTile = currentTile;
            RemoveItemTile();
            OnDropItemOnGround?.Invoke(tempTile);
        }

        public bool IsOccupied() => currentTile != null;

        public ItemInventoryTile GetItemTile() => currentTile;
    }
}