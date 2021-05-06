using System;
using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory
{
    public class BackPackSlotManager : PanelContainer
    {
        public Action<ItemInventoryTile> OnDropItemOnGround;
        public Action<ItemSlot> OnShowContextMenu;
        
        private BackPackSlot[,] backPackSlots;

        public override void _Ready()
        {
            backPackSlots = new BackPackSlot[4, 4];

            RegisterSlots();
        }

        /// <summary>
        /// Scan and register all inventory slots
        /// </summary>
        private void RegisterSlots()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    BackPackSlot slot = GetNode<BackPackSlot>($"Container/Rows/{row}/{col}");
                    slot.Init(row, col);
                    slot.OnDropItemOnGround += HandleOnDropItemOnGround;
                    slot.OnShowContextMenu += HandleOnShowContextMenu;
                    backPackSlots[row, col] = slot;
                }
            }
        }

        

        public bool AddItemTileToBackpack(ItemInventoryTile itemTile)
        {
            BackPackSlot slot = GetEmptySlot();
            
            // There aren't any open slots :'(
            if (slot == null) return false;
            
            slot.AddItemTile(itemTile);
            return true;
        }

        public void RemoveItemTileFromBackpack(ItemInventoryTile itemTile)
        {
            
        }

        /// <summary>
        /// Does the backpack have any empty slots?
        /// </summary>
        /// <returns>True of there is at least 1 empty slot</returns>
        public bool HasEmptySlots()
        {
            return GetEmptySlot() != null;
        }

        /// <summary>
        /// Get the first empty slot in the backpack
        /// </summary>
        private BackPackSlot GetEmptySlot()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (!backPackSlots[row, col].IsOccupied())
                    {
                        return backPackSlots[row, col];
                    }
                }
            }

            return null;
        }
        
        /// <summary>
        /// Forward event, override to do something specific.
        /// </summary>
        /// <param name="tile"></param>
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