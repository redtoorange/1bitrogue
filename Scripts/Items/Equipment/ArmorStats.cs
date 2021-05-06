using Godot;

namespace BitRoguelike.Scripts.Items.Equipment
{
    public class ArmorStats : ItemStats
    {
        [Export] public int armorBonus = 0;
        [Export] public int maxDexBonus = 10;
        [Export] public ArmorSlotType slotType = ArmorSlotType.HEAD;
    }
}