using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class PlayerUI : Node
    {
        [Export] private NodePath healthBarPath;
        [Export] private NodePath manaBarPath;

        private HealthBar healthBar;
        private ManaBar manaBar;

        public override void _Ready()
        {
            healthBar = GetNode<HealthBar>(healthBarPath);
            manaBar = GetNode<ManaBar>(manaBarPath);
        }

        public void Init(Player player)
        {
            healthBar.Init(player.GetHealthController());
            manaBar.Init(player.GetManaController());
        }
    }
}