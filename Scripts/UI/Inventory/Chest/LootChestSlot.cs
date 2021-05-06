using System;
using BitRoguelike.Scripts.UI.Inventory.Slots;

namespace BitRoguelike.Scripts.UI.Inventory.Chest
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