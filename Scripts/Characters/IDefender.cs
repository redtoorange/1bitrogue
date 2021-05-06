using BitRoguelike.Scripts.Characters.Controllers;

namespace BitRoguelike.Scripts.Characters
{
    public interface IDefender
    {
        ArmorController GetArmorController();
        int GetArmorClass();
        void TakeDamage(int amount);
    }
}