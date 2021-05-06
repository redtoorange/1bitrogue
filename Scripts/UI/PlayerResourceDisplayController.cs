using BitRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace BitRoguelike.Scripts.UI
{
    public class PlayerResourceDisplayController : Node
    {
        [Export] private NodePath healthBarPath = null;
        [Export] private NodePath manaBarPath = null;

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