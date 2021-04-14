using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class ArmorSlot : ItemSlot
    {
        [Export] private Items.ArmorSlot armorSlotType = Items.ArmorSlot.HEAD;
    }
}