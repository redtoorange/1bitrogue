using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.Items
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

    public class Weapon : Item, Equipable
    {
        private WeaponStats weaponStats;

        public override void _Ready()
        {
            base._Ready();

            weaponStats = stats as WeaponStats;
            if (weaponStats == null)
            {
                GD.PrintErr("Failed to convert ItemStat to WeaponStats.");
            }
        }

        public WeaponStats GetStats() => weaponStats;

        public int GetDamage() => weaponStats.maxDamage;
    }
}