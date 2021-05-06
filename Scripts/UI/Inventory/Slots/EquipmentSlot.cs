using System;
using BitRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace BitRoguelike.Scripts.UI.Inventory.Slots
{
    public abstract class EquipmentSlot : ItemSlot
    {
        public Action<EquipPayload> OnEquip;
        public Action<EquipPayload> OnUnequip;

        public override void AddItemTile(ItemInventoryTile tile)
        {
            GD.Print("EquipmentSlot - AddItemTile");
            base.AddItemTile(tile);
            OnEquip?.Invoke(GetPayload());
        }

        public override void RemoveItemTile()
        {
            GD.Print("EquipmentSlot - RemoveItemTile");
            OnUnequip?.Invoke(GetPayload());
            base.RemoveItemTile();
        }

        protected abstract EquipPayload GetPayload();
    }
}