using BitRoguelike.Scripts.Characters.Controllers;

namespace BitRoguelike.Scripts.Characters
{
    public interface IAttacker
    {
        WeaponController GetWeaponController();
        int GetHitBonus();
        int GetWeapon();
        int GetDamageBonus();
    }
}