using System;
using BitRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Chest
{
    public class LootChestSlot : ItemSlot
    {
        public static Action<ItemInventoryTile> OnLootChestItemTaken;
        
        public override bool CanDropDnDItem(ItemInventoryTile tile)
        {
            GD.Print("LootChestSlot - CanDropDnDItem");
            return false;
        }

        /// <summary>
        /// <inheritdoc />
        /// <br/>
        /// <br/>
        /// Invokes OnLootChestItemRemoved Action, removing the item from the chest it originate in.
        /// </summary>
        public override void RemoveItemTile()
        {
            GD.Print("LootChestSlot - RemoveItemTile");
            OnLootChestItemTaken?.Invoke(currentTile);
            base.RemoveItemTile();
        }

        /// <summary>
        /// Remove the ItemTile without notifying the original chest.
        /// </summary>
        public void RemoveItemTileNoPropagate()
        {
            GD.Print("LootChestSlot - RemoveItemTileNoPropagate");
            base.RemoveItemTile();
        }
    }
}