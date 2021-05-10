using System;
using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public class HealthController : ResourceController
    {
        public Action<HealthController> OnDie;

        [Export] private int maxHealth = 100;
        private int currentHealth;

        public override void _EnterTree()
        {
            currentHealth = maxHealth;
        }

        public void Heal(int amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnResourceChange?.Invoke(new ResourceChangeData(
                ResourceChangeType.GAIN,
                currentHealth,
                maxHealth));
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnResourceChange?.Invoke(new ResourceChangeData(
                ResourceChangeType.LOSE,
                currentHealth,
                maxHealth));

            if (currentHealth <= 0)
            {
                OnDie?.Invoke(this);
            }
        }

        public override int GetValue()
        {
            return currentHealth;
        }

        public override float GetPercentValue()
        {
            return currentHealth / (float) maxHealth;
        }
    }
}