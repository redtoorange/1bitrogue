using System.Collections.Generic;
using GameboyRoguelike.Scripts.Items;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class InventoryController : Node
    {
        private ArmorController armorController;
        private WeaponController weaponController;

        private List<Item> managedItems;

        public override void _Ready()
        {
            managedItems = new List<Item>();

            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i) is Item item)
                {
                    managedItems.Add(item);
                }
            }
        }

        public void Init(ArmorController armorController, WeaponController weaponController)
        {
            this.armorController = armorController;
            this.weaponController = weaponController;
        }

        public void AddItem(Item itemToAdd)
        {
        }

        public void RemoveItem(Item itemToRemove)
        {
        }

        public List<Item> GetItems() => managedItems;
    }
}