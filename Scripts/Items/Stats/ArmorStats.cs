using Godot;

namespace GameboyRoguelike.Scripts.Items.Stats
{
    public class ArmorStats : ItemStats
    {
        [Export] public int armorBonus = 0;
        [Export] public int maxDexBonus = 10;
        [Export] public ArmorSlot slot = ArmorSlot.HEAD;
    }
}