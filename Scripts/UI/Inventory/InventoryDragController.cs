using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class InventoryDragController : Control
    {
        private EquipmentSlotsManager equipmentSlotsManager;
        private BackPackSlotManager backPackSlotManager;

        public void Init(EquipmentSlotsManager equipmentSlotsManager, BackPackSlotManager backPackSlotManager)
        {
            this.equipmentSlotsManager = equipmentSlotsManager;
            this.backPackSlotManager = backPackSlotManager;
        }
    }
}