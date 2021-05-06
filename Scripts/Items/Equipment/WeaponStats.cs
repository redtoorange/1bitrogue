using Godot;

namespace BitRoguelike.Scripts.Items.Equipment
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