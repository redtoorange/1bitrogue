using Godot;

namespace BitRoguelike.Scripts.Items.Equipment
{
    public enum ArmorSlotType
    {
        HEAD,
        CHEST,
        HANDS,
        LEGS
    }
    
    public class Armor : Item, IEquipable
    {
        private ArmorStats armorStats;

        public override void _Ready()
        {
            base._Ready();

            if (stats is ArmorStats c)
            {
                armorStats = c;
            }
            else
            {
                GD.PrintErr("Failed to convert ItemStat to ArmorStats.");
            }
        }

        public ArmorStats GetStats() => armorStats;
    }
}