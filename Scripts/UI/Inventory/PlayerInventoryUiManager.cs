using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PlayerInventoryUiManager : Control
    {
        public static PlayerInventoryUiManager S;
        
        [Export] private NodePath equipmentSlotsManagerPath;
        [Export] private NodePath backPackSlotManagerPath;
        [Export] private NodePath inventoryDragControllerPath;

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


        public override void _EnterTree()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                GD.PrintErr("Error: More than one PlayerInventoryManager in the scene tree");
            }
        }

        public override void _ExitTree()
        {
            S = null;
        }
    }
}