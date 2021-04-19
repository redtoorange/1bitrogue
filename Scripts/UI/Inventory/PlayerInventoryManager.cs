using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PlayerInventoryManager : Control
    {

        private EquipmentSlotsManager equipmentSlotsManager;
        private InventorySlotsManager inventorySlotsManager;
        public override void _Process(float delta)
        {
            equipmentSlotsManager = GetNode<EquipmentSlotsManager>("EquipmentSlots");
            inventorySlotsManager = GetNode<InventorySlotsManager>("InventorySlots");
            
            if (Input.IsActionJustPressed("Inventory"))
            {
                Visible = !Visible;
            }
        }
    }
}
