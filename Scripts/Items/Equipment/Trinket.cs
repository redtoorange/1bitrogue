using Godot;

namespace BitRoguelike.Scripts.Items.Equipment
{
    public enum TrinketSlotType
    {
        LEFT_RING,
        RIGHT_RING,
        NECKLACE
    }

    public enum TrinketEquipType
    {
        RING,
        NECKLACE,
    }

    public class Trinket : Item, IEquipable
    {
        private TrinketStats trinketStats;

        public override void _Ready()
        {
            base._Ready();

            if (stats is TrinketStats c)
            {
                trinketStats = c;
            }
            else
            {
                GD.PrintErr("Failed to convert ItemStat to TrinketStats.");
            }
        }

        public TrinketStats GetStats() => trinketStats;
    }
}