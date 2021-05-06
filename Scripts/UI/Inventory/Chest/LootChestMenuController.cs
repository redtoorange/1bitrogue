using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Items;
using BitRoguelike.Scripts.Map.Objects;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Chest
{
    public class LootChestMenuController : Control
    {
        [Export] private NodePath rowContainerPath = null;
        [Export] private NodePath lootAllButtonPath = null;
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
            LootChestSlot.OnLootChestItemTaken += HandleOnChestItemTaken;
        }

        public override void _ExitTree()
        {
            LootChestSlot.OnLootChestItemTaken -= HandleOnChestItemTaken;
        }

        private void HandleOnLootAllPressed()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    LootChestSlot slot = chestInventorySlots[row, col];
                    if (slot.IsOccupied() && backPackSlotManager.HasEmptySlots())
                    {
                        // Take ItemTile from Chest
                        ItemInventoryTile tile = slot.GetItemTile();
                        slot.RemoveItemTile();

                        // Add to the Backpack and to the inventory
                        backPackSlotManager.AddItemTileToBackpack(tile);
                        inventoryController.AddItem(tile.GetParentItem(), false);
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

        /// <summary>
        /// Unload the ItemInventoryTiles in the LootChestMenu, this should NOT remove the item from the original chest
        /// </summary>
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
                        slot.RemoveItemTileNoPropagate();   // Do not remove the Item from the Chest!!
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
            Node rowContainer = GetNode(rowContainerPath);
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    LootChestSlot slot = rowContainer.GetNode<LootChestSlot>($"{row}/{col}");
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

        /// <summary>
        /// This adds the item directly to the player's inventory without adding it to the UI.  It assumes the player
        /// dragged the Item into their inventory.
        /// </summary>
        private void HandleOnChestItemTaken(ItemInventoryTile tile)
        {
            Item item = tile.GetParentItem();
            currentChest.RemoveItem(item);
            inventoryController.AddItem(item, false);
        }
    }
}