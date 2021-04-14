using GameboyRoguelike.Scripts.Characters.Controllers;

namespace GameboyRoguelike.Scripts.Characters
{
    public interface IDefender
    {
        ArmorController GetArmorController();
        int GetArmorClass();
        void TakeDamage(int amount);
    }
}