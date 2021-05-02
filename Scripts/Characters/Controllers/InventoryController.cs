using System.Collections.Generic;
using GameboyRoguelike.Scripts.Characters.Player;
using GameboyRoguelike.Scripts.Items;
using GameboyRoguelike.Scripts.UI.Inventory;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
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
                    managedItems.Add(item);
                    playerInventoryUiManager.AddItemToUi(item);
                    item.SetCollisionEnabled(false);
                    item.SetSpriteEnabled(false);
                }
            }
        }

        /// <summary>
        /// Attempt to add an item to the inventory
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <returns>true if item was added, false otherwise</returns>
        public bool AddItem(Item itemToAdd)
        {
            if (!managedItems.Contains(itemToAdd))
            {
                AddChild(itemToAdd);
                managedItems.Add(itemToAdd);
                playerInventoryUiManager.AddItemToUi(itemToAdd);
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
            if (managedItems.Contains(itemToRemove))
            {
                managedItems.Remove(itemToRemove);
                RemoveChild(itemToRemove);
            }
        }

        public List<Item> GetItems() => managedItems;
    }
}