using Godot;

namespace BitRoguelike.Scripts.Items.Equipment
{
    public enum WeaponDamageType
    {
        SLASHING,
        PIERCING,
        BLUNT
    }

    public enum WeaponType
    {
        MELEE,
        RANGED
    }

    public enum WeaponSlot
    {
        MAIN_HAND,
        OFF_HAND
    }
    public enum WeaponEquipType
    {
        MAIN_HAND,
        OFF_HAND,
        EITHER_HAND,
        TWO_HANDS
    }

    public class Weapon : Item, IEquipable
    {
        private WeaponStats weaponStats;

        public override void _Ready()
        {
            base._Ready();

            if (stats is WeaponStats c)
            {
                weaponStats = c;
            }
            else
            {
                GD.PrintErr("Failed to convert ItemStat to WeaponStats.");
            }
        }

        public WeaponStats GetStats() => weaponStats;

        public int GetDamage() => weaponStats.maxDamage;
    }
}