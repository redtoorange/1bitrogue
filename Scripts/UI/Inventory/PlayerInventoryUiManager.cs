using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PlayerInventoryUiManager : Control
    {
        public static PlayerInventoryUiManager S;

        private Player player;
        private EquipmentSlotsManager equipmentSlotsManager;
        private InventorySlotsManager inventorySlotsManager;
        private InventoryDragController inventoryDragController;

        public override void _Ready()
        {
            equipmentSlotsManager = GetNode<EquipmentSlotsManager>("EquipmentSlots");
            inventorySlotsManager = GetNode<InventorySlotsManager>("InventorySlots");
            inventoryDragController = GetNode<InventoryDragController>("DragController");

            // Inject Dependencies
            equipmentSlotsManager.Init();
            inventorySlotsManager.Init();
            inventoryDragController.Init(equipmentSlotsManager, inventorySlotsManager);
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