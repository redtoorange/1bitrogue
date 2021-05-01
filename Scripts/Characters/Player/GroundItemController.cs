using System.Collections.Generic;
using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Items;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class GroundItemController : Node
    {
        private List<Item> itemsInSpace;
        private InventoryController inventoryController;

        public override void _Ready()
        {
            itemsInSpace = new List<Item>();
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("Pickup"))
            {
                PickupAll();
            }
        }

        public void Init(InventoryController inventoryController)
        {
            this.inventoryController = inventoryController;
        }


        public void AddItem(Item item)
        {
            GD.Print("Attempting to add " + item.Name + " to itemsInSpace");
            if (!itemsInSpace.Contains(item))
            {
                GD.Print("Added " + item.Name + " to itemsInSpace");
                itemsInSpace.Add(item);
            }
        }

        public void RemoveItem(Item item)
        {
            GD.Print("Attempting to remove " + item.Name + " from itemsInSpace");
            if (itemsInSpace.Contains(item))
            {
                GD.Print("Removed " + item.Name + " from itemsInSpace");
                itemsInSpace.Remove(item);
            }
        }

        public void PickupAll()
        {
            for (int i = itemsInSpace.Count - 1; i >= 0; i--)
            {
                Item item = itemsInSpace[i];

                if (inventoryController.CanPickup(item))
                {
                    item.OnPickedUp();
                    inventoryController.AddItem(item);
                }
            }
        }
    }
}