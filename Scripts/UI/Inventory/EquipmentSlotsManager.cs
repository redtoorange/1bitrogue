using GameboyRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class EquipmentSlotsManager : PanelContainer
    {
        [Export] private NodePath headSlotPath;
        [Export] private NodePath chestSlotPath;
        [Export] private NodePath legsSlotPath;
        [Export] private NodePath mainHandSlotPath;
        [Export] private NodePath offHandSlotPath;
    
        private ArmorSlot head;
        private ArmorSlot chest;
        private ArmorSlot legs;

        private WeaponSlot mainHand;
        private WeaponSlot offHand;

        public override void _Ready()
        {
            head = GetNode<ArmorSlot>(headSlotPath);
            chest = GetNode<ArmorSlot>(chestSlotPath);
            legs = GetNode<ArmorSlot>(legsSlotPath);
        
            mainHand = GetNode<WeaponSlot>(mainHandSlotPath);
            offHand = GetNode<WeaponSlot>(offHandSlotPath);
        }
    }
}
