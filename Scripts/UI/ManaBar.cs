using GameboyRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class ManaBar : TextureProgress
    {
        private ManaController manaController;

        public void Init(ManaController manaController)
        {
            this.manaController = manaController;
            
            this.manaController.Connect(
                nameof(ManaController.OnValueChange),
                this,
                nameof(SetValue));
            Value = this.manaController.GetMana();
        }

        public void SetValue(int amount)
        {
            Value = amount;
        }
    }
}