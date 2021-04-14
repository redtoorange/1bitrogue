using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class HealthController : Node
    {
        [Signal] public delegate void OnHealthChange(int amount);

        [Signal] public delegate void OnDeath();

        [Export] private int maxHealth = 100;

        private int currentHealth;

        public override void _Ready()
        {
            currentHealth = maxHealth;
        }

        public void HealDamage(int amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            EmitSignal(nameof(OnHealthChange), currentHealth);
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            EmitSignal(nameof(OnHealthChange), currentHealth);

            if (currentHealth <= 0)
            {
                EmitSignal(nameof(OnDeath));
            }
        }

        public int GetHealth()
        {
            return currentHealth;
        }
    }
}