using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory
{
    public class InventoryDragController : Control
    {
        private EquipmentSlotsManager equipmentSlotsManager;
        private BackPackSlotManager backPackSlotManager;

        private DragStartPayload currentStartPayload = null;
        private bool canDrop = false;
        private Control preview = null;
        private ItemSlot hoveredSlot = null;

        public void Init(EquipmentSlotsManager equipmentSlotsManager, BackPackSlotManager backPackSlotManager)
        {
            this.equipmentSlotsManager = equipmentSlotsManager;
            this.backPackSlotManager = backPackSlotManager;
        }

        public override void _EnterTree()
        {
            ItemSlot.OnHoverStarted += HandleOnHoverStarted;
            ItemSlot.OnHoverEnded += HandleOnHoverEnded;
            ItemSlot.OnDragStarted += HandleOnDragStarted;
            ItemSlot.OnDragEnded += HandleOnDragEnded;
        }

        public override void _ExitTree()
        {
            ItemSlot.OnHoverStarted -= HandleOnHoverStarted;
            ItemSlot.OnHoverEnded -= HandleOnHoverEnded;
            ItemSlot.OnDragStarted -= HandleOnDragStarted;
            ItemSlot.OnDragEnded -= HandleOnDragEnded;
        }

        public override void _Process(float delta)
        {
            if (IsDragging() && preview != null)
            {
                UpdatePreview();
            }
        }

        private void CreatePreview()
        {
            if (preview != null)
            {
                preview.QueueFree();
            }
            
            preview = new Control();

            TextureRect image = new TextureRect();
            image.Texture = currentStartPayload.draggedTile.GetParentItem().GetUiTexture();
            image.RectPosition = new Vector2(-8, -8);

            preview.AddChild(image);
            AddChild(preview);
        }

        private void DestroyPreview()
        {
            if (preview != null)
            {
                RemoveChild(preview);
                preview.QueueFree();
                preview = null;
            }
        }

        private void UpdatePreview()
        {
            preview.SetGlobalPosition(GetGlobalMousePosition());
        }
        
        public override void _Input(InputEvent @event)
        {
            if (IsDragging() && hoveredSlot == null && @event is InputEventMouseButton imb)
            {
                if (imb.IsActionReleased("LeftClick"))
                {
                    // GD.Print("Release Drag");
                    HandleOnDragEnded(null);
                }
            }
        }

        private void HandleOnHoverStarted(ItemSlot slot)
        {
            // GD.Print("HoverStarted on " + slot.Name);
            hoveredSlot = slot;

            if (IsDragging())
            {
                canDrop = slot.CanDropDnDItem(currentStartPayload.draggedTile);
            }
        }

        

        private void HandleOnHoverEnded(ItemSlot slot)
        {
            // GD.Print("HoverEnded on " + slot.Name);
            if (slot == hoveredSlot)
            {
                // GD.Print("Cleared Hovered ");
                hoveredSlot = null;
            }
        }

        private void HandleOnDragStarted(DragStartPayload payload)
        {
            currentStartPayload = payload;
            CreatePreview();
        }

        private void HandleOnDragEnded(DragStopPayload payload)
        {
            if (!IsDragging()) return;

            ItemSlot origin = currentStartPayload.originatingSlot;
            ItemInventoryTile tile = currentStartPayload.draggedTile;
            ItemSlot destination = payload?.destinationSlot;

            if (payload != null && destination != origin && canDrop)
            {
                if (destination.IsOccupied())
                {
                    origin.RemoveItemTile();

                    ItemInventoryTile tempTile = destination.GetItemTile();
                    destination.RemoveItemTile();

                    origin.AddItemTile(tempTile);
                    destination.DropDnDItem(tile);
                }
                else
                {
                    origin.RemoveItemTile();
                    destination.DropDnDItem(tile);
                }
            }
            else
            {
                origin.CancelDnD();
            }

            currentStartPayload = null;
            DestroyPreview();
        }

        private bool IsDragging() => currentStartPayload != null;
    }
}