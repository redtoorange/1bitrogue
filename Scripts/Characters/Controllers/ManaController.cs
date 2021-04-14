using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class ManaController : Node
    {
        [Signal] public delegate void OnValueChange(int newValue);

        [Export] private int maxMana = 100;

        private int currentMana;

        public override void _Ready()
        {
            currentMana = maxMana;
        }

        public void GainMana(int amount)
        {
            currentMana += amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            EmitSignal(nameof(OnValueChange), currentMana);
        }

        public void UseMana(int amount)
        {
            currentMana -= amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            EmitSignal(nameof(OnValueChange), currentMana);
        }

        public int GetMana()
        {
            return currentMana;
        }
    }
}