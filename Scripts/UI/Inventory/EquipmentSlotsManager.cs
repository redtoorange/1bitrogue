using System;
using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory
{
    public class EquipmentSlotsManager : PanelContainer
    {
        public Action<ItemInventoryTile> OnDropItemOnGround;
        public Action<ItemSlot> OnShowContextMenu;
        
        // Armor
        [Export] private NodePath headSlotPath = null;
        [Export] private NodePath chestSlotPath = null;
        [Export] private NodePath legsSlotPath = null;
        [Export] private NodePath handsSlotPath = null;
        
        // Weapons
        [Export] private NodePath mainHandSlotPath = null;
        [Export] private NodePath offHandSlotPath = null;

        // Trinkets
        [Export] private NodePath leftRingSlotPath = null;
        [Export] private NodePath necklaceSlotPath = null;
        [Export] private NodePath rightRingSlotPath = null;
        
        private ArmorSlot headSlot;
        private ArmorSlot chestSlot;
        private ArmorSlot legsSlot;
        private ArmorSlot handsSlot;

        private WeaponSlot mainHandSlot;
        private WeaponSlot offHandSlot;
        
        private TrinketSlot leftRingSlot;
        private TrinketSlot necklaceSlot;
        private TrinketSlot rightRingSlot;

        public override void _Ready()
        {
            headSlot = GetNode<ArmorSlot>(headSlotPath);
            chestSlot = GetNode<ArmorSlot>(chestSlotPath);
            legsSlot = GetNode<ArmorSlot>(legsSlotPath);
            handsSlot = GetNode<ArmorSlot>(handsSlotPath);
        
            mainHandSlot = GetNode<WeaponSlot>(mainHandSlotPath);
            offHandSlot = GetNode<WeaponSlot>(offHandSlotPath);
            
            leftRingSlot = GetNode<TrinketSlot>(leftRingSlotPath);
            necklaceSlot = GetNode<TrinketSlot>(necklaceSlotPath);
            rightRingSlot = GetNode<TrinketSlot>(rightRingSlotPath);
            
            ConnectCallbacks();
        }

        private void ConnectCallbacks()
        {
            ItemSlot[] slots = new ItemSlot[]
            {
                headSlot, chestSlot, legsSlot, handsSlot, mainHandSlot, offHandSlot, leftRingSlot, necklaceSlot, rightRingSlot
            };
            foreach (ItemSlot slot in slots)
            {
                slot.OnDropItemOnGround += HandleOnDropItemOnGround;
                slot.OnShowContextMenu += HandleOnShowContextMenu;
            }
        }

        public virtual void HandleOnDropItemOnGround(ItemInventoryTile tile)
        {
            OnDropItemOnGround?.Invoke(tile);
        }
        
        private void HandleOnShowContextMenu(ItemSlot slot)
        {
            OnShowContextMenu?.Invoke(slot);
        }
    }
}
