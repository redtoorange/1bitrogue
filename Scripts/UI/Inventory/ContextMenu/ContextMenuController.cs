using System;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Items.Consumable;
using BitRoguelike.Scripts.Items.Equipment;
using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.ContextMenu
{
    public class ContextMenuController : Control
    {
        public Action<ItemSlot, ItemUseType> OnItemUsed;
        
        [Export] private NodePath buttonContainerPath = null;
        [Export] private NodePath backDropMaskPath = null;
        [Export] private NodePath equipButtonPath = null;
        [Export] private NodePath consumeButtonPath = null;
        [Export] private NodePath inspectButtonPath = null;
        [Export] private NodePath dropButtonPath = null;

        private VBoxContainer buttonContainer;
        private MaskingBackdropHandler backDropMask;
        private Button equipButton;
        private Button consumeButton;
        private Button inspectButton;
        private Button dropButton;

        private ItemSlot currentSlot;
        private ItemInventoryTile currentTile;
        private Item currentItem;

        private ItemUseType currentUseType = ItemUseType.EQUIP;

        public override void _Ready()
        {
            buttonContainer = GetNode<VBoxContainer>(buttonContainerPath);
            backDropMask = GetNode<MaskingBackdropHandler>(backDropMaskPath);
            
            equipButton = GetNode<Button>(equipButtonPath);
            consumeButton = GetNode<Button>(consumeButtonPath);
            inspectButton = GetNode<Button>(inspectButtonPath);
            dropButton = GetNode<Button>(dropButtonPath);
            
            ConnectSignals();
        }

        public override void _Input(InputEvent @event)
        {
            if (!IsVisibleInTree()) return;
            
            if (@event is InputEventKey key)
            {
                if (key.IsActionPressed("Back"))
                {
                    HideMenu();
                    GetTree().SetInputAsHandled();
                }
                else if (key.IsActionPressed("Inventory"))
                {
                    HideMenu();
                }
            }
            
            if (@event is InputEventMouseButton mb && mb.Pressed)
            {
                if (!buttonContainer.GetRect().HasPoint(mb.Position))
                {
                    HideMenu();
                }
            }
        }

        public void ShowMenu(ItemSlot itemSlot)
        {
            currentSlot = itemSlot;
            currentTile = itemSlot.GetItemTile();
            if (currentTile == null)
            {
                HideMenu();
                return;
            }

            currentItem = currentTile.GetParentItem();
            if (currentItem == null)
            {
                HideMenu();
                GD.PrintErr("Null Item?");
                return;
            }

            SelectButtons();
            PositionContainer();
            
            Visible = true;
        }

        private void SelectButtons()
        {
            int buttons = 2;
            if (currentItem is IEquipable)
            {
                equipButton.Visible = true;
                UpdateEquipText();
                buttons++;
            }
            else
            {
                equipButton.Visible = false;
            }
            
            if (currentItem is IConsumable)
            {
                consumeButton.Visible = true;
                UpdateConsumeText();
                buttons++;
            }
            else
            {
                consumeButton.Visible = false;
            }

            Vector2 size = buttonContainer.RectSize;
            size.y = buttons * 16;
            buttonContainer.RectSize = size;
        }

        private void UpdateEquipText()
        {
            if (currentSlot is BackPackSlot)
            {
                equipButton.Text = "Equip";
                currentUseType = ItemUseType.EQUIP;
            }
            else
            {
                equipButton.Text = "Remove";
                currentUseType = ItemUseType.UNEQUIP;
            }
        }

        private void UpdateConsumeText()
        {
            IConsumable consumable = currentItem as IConsumable;
            consumeButton.Text = consumable.GetConsumeText();
            currentUseType = ItemUseType.CONSUME;
        }

        private void PositionContainer()
        {
            buttonContainer.RectPosition = currentSlot.RectGlobalPosition + new Vector2(8, 16);

            float panelExtent = buttonContainer.RectPosition.x + buttonContainer.RectSize.x;
            float displayAreaSize = RectSize.x;
            if (panelExtent > displayAreaSize)
            {
                float offset = panelExtent - displayAreaSize;
                Vector2 position = buttonContainer.RectPosition;
                position.x -= offset;
                buttonContainer.RectPosition = position;
            }

            backDropMask.SetMaskLocation(currentSlot.RectGlobalPosition);
        }

        public void HideMenu()
        {
            Visible = false;

            currentSlot = null;
            currentTile = null;
            currentItem = null;
        }

        private void ConnectSignals()
        {
            equipButton.Connect("pressed", this, nameof(OnEquipPressed));
            consumeButton.Connect("pressed", this, nameof(OnConsumePressed));
            inspectButton.Connect("pressed", this, nameof(OnInspectPressed));
            dropButton.Connect("pressed", this, nameof(OnDropPressed));
        }

        private void OnEquipPressed()
        {
            GD.Print("Context Menu Equip Pressed");
            OnItemUsed?.Invoke(currentSlot, currentUseType);
            HideMenu();
        }
        
        private void OnConsumePressed()
        {
            GD.Print("Context Menu Consume Pressed");
            OnItemUsed?.Invoke(currentSlot, currentUseType);
            HideMenu();
        }
        
        private void OnInspectPressed()
        {
            GD.Print("Context Menu Inspect Pressed");
        }
        
        private void OnDropPressed()
        {
            currentSlot.ContextMenuNotifyDrop();
            HideMenu();
        }
    }
}