using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.Items
{
    public enum ArmorSlot
    {
        HEAD,
        CHEST,
        LEGS
    }
    
    public class Armor : Item, Equipable
    {
        private ArmorStats armorStats;

        public override void _Ready()
        {
            base._Ready();

            armorStats = stats as ArmorStats;
            if (armorStats == null)
            {
                GD.PrintErr("Failed to convert ItemStat to ArmorStats.");
            }
        }

        public ArmorStats GetStats() => armorStats;
    }
}