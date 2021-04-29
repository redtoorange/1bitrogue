using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class InventoryDragController : Control
    {
        private EquipmentSlotsManager equipmentSlotsManager;
        private InventorySlotsManager inventorySlotsManager;

        public void Init(EquipmentSlotsManager equipmentSlotsManager, InventorySlotsManager inventorySlotsManager)
        {
            this.equipmentSlotsManager = equipmentSlotsManager;
            this.inventorySlotsManager = inventorySlotsManager;
        }
    }
}