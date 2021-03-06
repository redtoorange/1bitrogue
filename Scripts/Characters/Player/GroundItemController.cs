using System.Collections.Generic;
using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Items;
using Godot;

namespace BitRoguelike.Scripts.Characters.Player
{
    public class GroundItemController : Node2D
    {
        private List<Item> itemsInSpace;
        
        private InventoryController inventoryController;
        private Player player;
        
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

        public void Init(Player player, InventoryController inventoryController)
        {
            this.player = player;
            this.inventoryController = inventoryController;
        }


        public void AddItem(Item item)
        {
            if (!itemsInSpace.Contains(item))
            {
                itemsInSpace.Add(item);
            }
        }

        public void RemoveItem(Item item)
        {
            if (itemsInSpace.Contains(item))
            {
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
                    item.PickUpItem();
                    inventoryController.AddItem(item, true);
                }
            }
        }
        
        public void DropItemOnGround(Item item)
        {
            item.DropItem(player.Position);
        }
    }
}