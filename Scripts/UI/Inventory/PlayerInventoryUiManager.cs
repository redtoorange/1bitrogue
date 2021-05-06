using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Map.Objects;
using BitRoguelike.Scripts.UI.Inventory.Chest;
using BitRoguelike.Scripts.UI.Inventory.ContextMenu;
using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory
{
    public enum ItemUseType
    {
        EQUIP,
        UNEQUIP,
        CONSUME
    }
    public class PlayerInventoryUiManager : Control
    {
        // public static PlayerInventoryUiManager S;
        
        [Export] private NodePath equipmentSlotsManagerPath = null;
        [Export] private NodePath backPackSlotManagerPath = null;
        [Export] private NodePath inventoryDragControllerPath = null;
        [Export] private NodePath chestMenuControllerPath = null;
        
        [Export] private PackedScene itemInventoryTilePrefab = null;

        // Children
        private EquipmentSlotsManager equipmentSlotsManager;
        private BackPackSlotManager backPackSlotManager;
        private InventoryDragController inventoryDragController;
        private ContextMenuController contextMenuController;
        private LootChestMenuController lootChestMenuController;
        
        // Injected
        private InventoryController inventoryController;

        public override void _Ready()
        {
            equipmentSlotsManager = GetNode<EquipmentSlotsManager>(equipmentSlotsManagerPath);
            backPackSlotManager = GetNode<BackPackSlotManager>(backPackSlotManagerPath);
            inventoryDragController = GetNode<InventoryDragController>(inventoryDragControllerPath);
            lootChestMenuController = GetNode<LootChestMenuController>(chestMenuControllerPath);

            backPackSlotManager.OnDropItemOnGround += HandleOnDropItem;
            backPackSlotManager.OnShowContextMenu += HandleOnShowContextMenu;

            equipmentSlotsManager.OnDropItemOnGround += HandleOnDropItem;
            equipmentSlotsManager.OnShowContextMenu += HandleOnShowContextMenu;
        }

        public void Init(InventoryController inventoryController, ContextMenuController contextMenuController)
        {
            this.inventoryController = inventoryController;
            this.contextMenuController = contextMenuController;
            
            contextMenuController.OnItemUsed += HandleOnItemUse;
            
            inventoryDragController.Init(equipmentSlotsManager, backPackSlotManager);
            equipmentSlotsManager.Init(inventoryController);
            lootChestMenuController.Init(itemInventoryTilePrefab, this.inventoryController, backPackSlotManager);
        }

        public void AddItemToUi(Item item)
        {
            GD.Print("PlayerInventoryUiManager - AddItemToUi");
            ItemInventoryTile tile = itemInventoryTilePrefab.Instance<ItemInventoryTile>();
            tile.Init(item);
            
            backPackSlotManager.AddItemTileToBackpack(tile);
        }

        public void HandleOnDropItem(ItemInventoryTile tile)
        {
            GD.Print("PlayerInventoryUiManager - HandleOnDropItem");
            Item droppedItem = tile.GetParentItem();
            inventoryController.RemoveItem(droppedItem);
            tile.QueueFree();
        }

        public void HandleOnShowContextMenu(ItemSlot slot)
        {
            GD.Print("PlayerInventoryUiManager - HandleOnShowContextMenu");
            contextMenuController.ShowMenu(slot);
        }

        public void OpenChest(LootChest chest)
        {
            GD.Print("PlayerInventoryUiManager - OpenChest");
            lootChestMenuController.LoadChest(chest);
            equipmentSlotsManager.Visible = false;
            lootChestMenuController.Visible = true;
        }

        public void CloseChest()
        {
            GD.Print("PlayerInventoryUiManager - CloseChest");
            lootChestMenuController.UnloadChest();
            equipmentSlotsManager.Visible = true;
            lootChestMenuController.Visible = false;
        }

        public void HandleOnItemUse(ItemSlot slot, ItemUseType useType)
        {
            if (useType == ItemUseType.CONSUME)
            {
                // TODO
            }
            else if(useType == ItemUseType.EQUIP)
            {
                // TODO need to handle swapping
                ItemInventoryTile tile = slot.GetItemTile();
                slot.RemoveItemTile();

                equipmentSlotsManager.AddItemTileToEquipment(tile);
            }
            else if (useType == ItemUseType.UNEQUIP)
            {
                // TODO need to handle full bag
                
                // The tile being removed will trigger the unequip, so just add it to the backpack
                ItemInventoryTile tile = slot.GetItemTile();
                slot.RemoveItemTile();

                backPackSlotManager.AddItemTileToBackpack(tile);
            }
        }
    }
}