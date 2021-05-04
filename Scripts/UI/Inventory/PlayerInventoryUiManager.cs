using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Characters.Player;
using GameboyRoguelike.Scripts.Items;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PlayerInventoryUiManager : Control
    {
        // public static PlayerInventoryUiManager S;
        
        [Export] private NodePath equipmentSlotsManagerPath;
        [Export] private NodePath backPackSlotManagerPath;
        [Export] private NodePath inventoryDragControllerPath;

        [Export] private PackedScene itemInventoryTilePrefab;

        // Children
        private EquipmentSlotsManager equipmentSlotsManager;
        private BackPackSlotManager backPackSlotManager;
        private InventoryDragController inventoryDragController;
        
        // Injected
        private InventoryController inventoryController;

        public override void _Ready()
        {
            equipmentSlotsManager = GetNode<EquipmentSlotsManager>(equipmentSlotsManagerPath);
            backPackSlotManager = GetNode<BackPackSlotManager>(backPackSlotManagerPath);
            inventoryDragController = GetNode<InventoryDragController>(inventoryDragControllerPath);

            // Inject Dependencies
            // equipmentSlotsManager.Init();
            // backPackSlotManager.Init();
            inventoryDragController.Init(equipmentSlotsManager, backPackSlotManager);

            backPackSlotManager.OnDropItemOnGround += HandleOnDropItem;
        }

        public void Init(InventoryController inventoryController)
        {
            this.inventoryController = inventoryController;
        }

        public void AddItemToUi(Item item)
        {
            ItemInventoryTile tile = itemInventoryTilePrefab.Instance<ItemInventoryTile>();
            tile.Init(item);
            
            backPackSlotManager.AddItemTileToBackpack(tile);
        }

        public void HandleOnDropItem(ItemInventoryTile tile)
        {
            Item droppedItem = tile.GetParentItem();
            inventoryController.RemoveItem(droppedItem);
            tile.QueueFree();
        }
    }
}