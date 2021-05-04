using System;
using System.Collections.Generic;
using System.IO.Ports;
using GameboyRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class EquipmentSlotsManager : PanelContainer
    {
        public Action<ItemInventoryTile> OnDropItemOnGround;
        public Action<ItemSlot> OnShowContextMenu;
        
        // Armor
        [Export] private NodePath headSlotPath;
        [Export] private NodePath chestSlotPath;
        [Export] private NodePath legsSlotPath;
        [Export] private NodePath handsSlotPath;
        
        // Weapons
        [Export] private NodePath mainHandSlotPath;
        [Export] private NodePath offHandSlotPath;
    
        // Trinkets
        [Export] private NodePath leftRingSlotPath;
        [Export] private NodePath necklaceSlotPath;
        [Export] private NodePath rightRingSlotPath;
        
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
