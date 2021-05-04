using System.Collections.Generic;
using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Items;
using GameboyRoguelike.Scripts.Map.Objects;
using GameboyRoguelike.Scripts.UI.Inventory;
using Godot;

namespace BitRoguelike.Scripts.UI.Chest
{
    public class LootChestMenuController : Control
    {
        [Export] private NodePath lootAllButtonPath;
        private Button lootAllButton;

        private BackPackSlotManager backPackSlotManager;
        private InventoryController inventoryController;
        private PackedScene itemInventoryTilePrefab;
        private LootChest currentChest = null;
        

        private LootChestSlot[,] chestInventorySlots;

        public override void _Ready()
        {
            chestInventorySlots = new LootChestSlot[4, 2];

            RegisterSlots();
            
            lootAllButton = GetNode<Button>(lootAllButtonPath);
            lootAllButton.Connect("pressed", this, nameof(HandleOnLootAllPressed));
        }

        public void Init(PackedScene itemInventoryTilePrefab, InventoryController inventoryController, BackPackSlotManager backPackSlotManager)
        {
            this.inventoryController = inventoryController;
            this.itemInventoryTilePrefab = itemInventoryTilePrefab;
            this.backPackSlotManager = backPackSlotManager;
        }

        public override void _EnterTree()
        {
            LootChestSlot.OnLootChestItemRemoved += HandleOnChestItemRemoved;
        }

        public override void _ExitTree()
        {
            LootChestSlot.OnLootChestItemRemoved -= HandleOnChestItemRemoved;
        }

        private void HandleOnLootAllPressed()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    LootChestSlot slot = chestInventorySlots[row, col];
                    if (slot.IsOccupied())
                    {
                        ItemInventoryTile tile = slot.GetItemTile();
                        slot.RemoveItemTile();
                        tile.QueueFree();
                    }
                }
            }
        }

        public void LoadChest(LootChest chest)
        {
            currentChest = chest;

            foreach (Item item in chest.GetContainedItems())
            {
                ItemInventoryTile tile = itemInventoryTilePrefab.Instance<ItemInventoryTile>();
                tile.Init(item);
                AddTileToSlot(tile);
            }
        }

        public void UnloadChest()
        {
            // Dispose of chest tiles
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    LootChestSlot slot = chestInventorySlots[row, col];
                    if (slot.IsOccupied())
                    {
                        ItemInventoryTile tile = slot.GetItemTile();
                        slot.RemoveItemTile();
                        tile.QueueFree();
                    }
                }
            }
            
            currentChest.Close();
            currentChest = null;
        }

        private void AddTileToSlot(ItemInventoryTile tile)
        {
            LootChestSlot slot = GetEmptySlot();
            slot.AddItemTile(tile);
        }
        
        /// <summary>
        /// Scan and register all inventory slots
        /// </summary>
        private void RegisterSlots()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    LootChestSlot slot = GetNode<LootChestSlot>($"Panel/Container/Rows/{row}/{col}");
                    chestInventorySlots[row, col] = slot;
                }
            }
        }
        
        private LootChestSlot GetEmptySlot()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    if (!chestInventorySlots[row, col].IsOccupied())
                    {
                        return chestInventorySlots[row, col];
                    }
                }
            }

            return null;
        }

        private void HandleOnChestItemRemoved(ItemInventoryTile tile)
        {
            Item item = tile.GetParentItem();
            currentChest.RemoveItem(item);
            inventoryController.AddItem(item, false);
        }
    }
}