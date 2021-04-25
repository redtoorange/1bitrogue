using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class ManaController : ResourceController
    {
        [Export] private int maxMana = 100;
        private int currentMana;

        public override void _EnterTree()
        {
            currentMana = maxMana;
        }

        public void GainMana(int amount)
        {
            if (amount < 1) return;

            currentMana += amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            OnResourceChange?.Invoke(new ResourceChangeData(
                ResourceChangeType.GAIN,
                currentMana,
                maxMana));
        }

        public void UseMana(int amount)
        {
            if (amount < 1) return;

            currentMana -= amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            OnResourceChange?.Invoke(new ResourceChangeData(
                ResourceChangeType.LOSE,
                currentMana,
                maxMana));
        }

        public override int GetValue()
        {
            return currentMana;
        }
        
        public override float GetPercentValue()
        {
            return currentMana / (float) maxMana;
        }
    }
}