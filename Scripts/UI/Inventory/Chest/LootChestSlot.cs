using System;
using GameboyRoguelike.Scripts.UI.Inventory;
using GameboyRoguelike.Scripts.UI.Inventory.Slots;

namespace BitRoguelike.Scripts.UI.Chest
{
    public class LootChestSlot : ItemSlot
    {
        public static Action<ItemInventoryTile> OnLootChestItemRemoved;
        
        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            return false;
        }

        public override void RemoveItemTile()
        {
            OnLootChestItemRemoved?.Invoke(currentTile);
            base.RemoveItemTile();
        }
    }
}