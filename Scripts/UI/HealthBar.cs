using GameboyRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class HealthBar : TextureProgress
    {
        private HealthController healthController;
        
        public void Init(HealthController healthController)
        {
            this.healthController = healthController;  
            
            this.healthController.Connect(
                nameof(HealthController.OnHealthChange),
                this,
                nameof(SetValue));
            
            Value =  this.healthController.GetHealth();
        }

        public void SetValue(int amount)
        {
            Value = amount;
        }
    }
}