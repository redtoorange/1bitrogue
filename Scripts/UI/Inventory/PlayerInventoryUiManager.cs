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

        private Player player;
        private EquipmentSlotsManager equipmentSlotsManager;
        private BackPackSlotManager backPackSlotManager;
        private InventoryDragController inventoryDragController;

        public override void _Ready()
        {
            equipmentSlotsManager = GetNode<EquipmentSlotsManager>(equipmentSlotsManagerPath);
            backPackSlotManager = GetNode<BackPackSlotManager>(backPackSlotManagerPath);
            inventoryDragController = GetNode<InventoryDragController>(inventoryDragControllerPath);

            // Inject Dependencies
            equipmentSlotsManager.Init();
            backPackSlotManager.Init();
            inventoryDragController.Init(equipmentSlotsManager, backPackSlotManager);
        }

        public void Init(Player player)
        {
            this.player = player;
        }

        public void AddItemToUi(Item item)
        {
            ItemInventoryTile tile = itemInventoryTilePrefab.Instance<ItemInventoryTile>();
            tile.Init(item);
            
            backPackSlotManager.AddItemTileToBackpack(tile);
        }
    }
}