using Godot;

namespace GameboyRoguelike.Scripts.Items.Stats
{
    public class WeaponStats : ItemStats
    {
        [Export] public WeaponType weaponType = WeaponType.MELEE;
        [Export] public WeaponDamageType weaponDamageType = WeaponDamageType.BLUNT;
        [Export] public WeaponEquipType weaponEquipType = WeaponEquipType.MAIN_HAND;
        [Export] public int minDamage;
        [Export] public int maxDamage;
    }
}