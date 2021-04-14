using GameboyRoguelike.Scripts.Characters.Controllers;

namespace GameboyRoguelike.Scripts.Characters
{
    public interface IAttacker
    {
        WeaponController GetWeaponController();
        int GetHitBonus();
        int GetWeapon();
        int GetDamageBonus();
    }
}