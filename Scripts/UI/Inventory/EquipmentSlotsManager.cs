using System;
using System.Collections.Generic;
using BitRoguelike.Scripts.Characters.Controllers;
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

        private EquipmentSlot[] slots;

        // Injected
        private InventoryController inventoryController;

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

            slots = new EquipmentSlot[]
            {
                headSlot, chestSlot, legsSlot, handsSlot, mainHandSlot, offHandSlot, leftRingSlot, necklaceSlot,
                rightRingSlot
            };
            ConnectCallbacks();
        }

        public void Init(InventoryController inventoryController)
        {
            this.inventoryController = inventoryController;
        }

        private void ConnectCallbacks()
        {
            foreach (EquipmentSlot slot in slots)
            {
                slot.OnDropItemOnGround += HandleOnDropItemOnGround;
                slot.OnShowContextMenu += HandleOnShowContextMenu;
                slot.OnEquip += HandleOnEquip;
                slot.OnUnequip += HandleOnUnEquip;
            }
        }

        public List<EquipmentSlot> FindCompatibleSlot(ItemInventoryTile tile)
        {
            List<EquipmentSlot> compatibleSlots = new List<EquipmentSlot>();
            
            foreach (EquipmentSlot slot in slots)
            {
                if (slot.CanDropDnDItem(tile))
                {
                    compatibleSlots.Add(slot);
                }
            }

            return compatibleSlots;
        }

        public void HandleOnEquip(EquipPayload payload)
        {
            GD.Print("EquipmentSlotsManager - HandleOnEquip");
            inventoryController.EquipItem(payload);
        }

        public void HandleOnUnEquip(EquipPayload payload)
        {
            GD.Print("EquipmentSlotsManager - HandleOnUnEquip");
            inventoryController.UnEquipItem(payload);
        }

        public virtual void HandleOnDropItemOnGround(ItemInventoryTile tile)
        {
            GD.Print("EquipmentSlotsManager - HandleOnDropItemOnGround");
            OnDropItemOnGround?.Invoke(tile);
        }

        private void HandleOnShowContextMenu(ItemSlot slot)
        {
            GD.Print("EquipmentSlotsManager - HandleOnShowContextMenu");
            OnShowContextMenu?.Invoke(slot);
        }
    }
}