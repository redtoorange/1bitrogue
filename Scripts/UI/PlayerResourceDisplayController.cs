using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class PlayerResourceDisplayController : Node
    {
        [Export] private NodePath healthBarPath;
        [Export] private NodePath manaBarPath;

        private ResourceBar healthBar;
        private ResourceBar manaBar;

        public override void _Ready()
        {
            healthBar = GetNode<ResourceBar>(healthBarPath);
            manaBar = GetNode<ResourceBar>(manaBarPath);
        }

        public void Init(HealthController healthController, ManaController manaController)
        {
            healthBar.Init(healthController);
            manaBar.Init(manaController);
        }
    }
}