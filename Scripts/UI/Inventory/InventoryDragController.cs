using GameboyRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class InventoryDragController : Control
    {
        private EquipmentSlotsManager equipmentSlotsManager;
        private BackPackSlotManager backPackSlotManager;

        private DragStartPayload currentStartPayload = null;
        private bool canDrop = false;
        private Control preview = null;

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

        private void HandleOnHoverStarted(ItemSlot slot)
        {
            if (IsDragging())
            {
                canDrop = slot.CanDropDnDItem(currentStartPayload.draggedTile);
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (IsDragging() && @event is InputEventMouseMotion mm)
            {
                // NOP
            }
        }

        private void HandleOnHoverEnded(ItemSlot slot)
        {
            // NOP
        }

        private void HandleOnDragStarted(DragStartPayload payload)
        {
            currentStartPayload = payload;
            CreatePreview();
        }

        private void HandleOnDragEnded(DragStopPayload payload)
        {
            if (!IsDragging()) return;

            ItemSlot destination = payload.destinationSlot;
            ItemSlot origin = currentStartPayload.originatingSlot;
            ItemInventoryTile tile = currentStartPayload.draggedTile;

            if (destination != origin && canDrop)
            {
                if (destination.IsOccupied())
                {
                    origin.RemoveItemTile(tile);

                    ItemInventoryTile tempTile = destination.GetItemTile();
                    destination.RemoveItemTile(tempTile);

                    origin.AddItemTile(tempTile);
                    destination.DropDnDItem(tile);
                }
                else
                {
                    origin.RemoveItemTile(tile);
                    destination.DropDnDItem(tile);
                }
            }
            else
            {
                origin.CancelDnD();
            }

            currentStartPayload = null;
            DestroyPreview();
            GD.Print($"Drag Ended on {payload.destinationSlot.Name}");
        }

        private bool IsDragging() => currentStartPayload != null;
    }
}