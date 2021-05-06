using System.Collections.Generic;
using BitRoguelike.Scripts.Characters.Player;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Items.Equipment;
using BitRoguelike.Scripts.UI.Inventory;
using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public class EquipPayload
    {
        public IEquipable equipable = null;

        public EquipPayload(IEquipable equipable)
        {
            this.equipable = equipable;
        }
    }

    public class WeaponEquipPayload : EquipPayload
    {
        public WeaponSlotType targetSlotType = WeaponSlotType.MAIN_HAND;

        public WeaponEquipPayload(IEquipable equipable, WeaponSlotType targetSlotType) : base(equipable)
        {
            this.targetSlotType = targetSlotType;
        }
    }

    public class ArmorEquipPayload : EquipPayload
    {
        public ArmorSlotType targetSlot = ArmorSlotType.HEAD;

        public ArmorEquipPayload(IEquipable equipable, ArmorSlotType targetSlot) : base(equipable)
        {
            this.targetSlot = targetSlot;
        }
    }

    public class TrinketEquipPayload : EquipPayload
    {
        public TrinketSlotType targetSlotType = TrinketSlotType.LEFT_RING;

        public TrinketEquipPayload(IEquipable equipable, TrinketSlotType targetSlotType) : base(equipable)
        {
            this.targetSlotType = targetSlotType;
        }
    }

    public class InventoryController : Node
    {
        private ArmorController armorController;
        private WeaponController weaponController;
        private GroundItemController groundItemController;
        private PlayerInventoryUiManager playerInventoryUiManager;

        private List<Item> managedItems;

        public override void _Ready()
        {
            managedItems = new List<Item>();
        }

        public void Init(
            ArmorController armorController,
            WeaponController weaponController,
            GroundItemController groundItemController,
            PlayerInventoryUiManager playerInventoryUiManager
        )
        {
            this.armorController = armorController;
            this.weaponController = weaponController;
            this.groundItemController = groundItemController;
            this.playerInventoryUiManager = playerInventoryUiManager;

            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i) is Item item)
                {
                    GD.Print("InventoryController - AddItem");
                    managedItems.Add(item);
                    playerInventoryUiManager.AddItemToUi(item);
                    item.SetEnabled(false);
                }
            }
        }

        /// <summary>
        /// Attempt to add an item to the inventory
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <returns>true if item was added, false otherwise</returns>
        public bool AddItem(Item itemToAdd, bool addToUi)
        {
            GD.Print("InventoryController - AddItem");
            if (!managedItems.Contains(itemToAdd))
            {
                AddChild(itemToAdd);
                managedItems.Add(itemToAdd);
                if (addToUi)
                {
                    playerInventoryUiManager.AddItemToUi(itemToAdd);
                }

                return true;
            }

            return false;
        }

        public bool CanPickup(Item itemToAdd)
        {
            return !managedItems.Contains(itemToAdd);
        }

        public void RemoveItem(Item itemToRemove)
        {
            GD.Print("InventoryController - RemoveItem");
            if (managedItems.Contains(itemToRemove))
            {
                managedItems.Remove(itemToRemove);
                RemoveChild(itemToRemove);
                groundItemController.DropItemOnGround(itemToRemove);
            }
        }

        public void UnEquipItem(EquipPayload payload)
        {
            GD.Print("InventoryController - UnEquipItem");
            if (payload is WeaponEquipPayload w)
            {
                weaponController.UnEquipWeapon(w);
            }
            else if (payload is ArmorEquipPayload a)
            {
                armorController.UnEquipArmor(a);
            }
            else if (payload is TrinketEquipPayload t)
            {
                // TODO   
            }
        }

        public void EquipItem(EquipPayload payload)
        {
            GD.Print("InventoryController - EquipItem");
            if (payload is WeaponEquipPayload w)
            {
                weaponController.EquipWeapon(w);
            }
            else if (payload is ArmorEquipPayload a)
            {
                armorController.EquipArmor(a);
            }
            else if (payload is TrinketEquipPayload t)
            {
                // TODO   
            }
        }

        public List<Item> GetItems() => managedItems;
    }
}