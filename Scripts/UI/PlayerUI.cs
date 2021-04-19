using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class PlayerUI : Node
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

        public void Init(Player player)
        {
            healthBar.Init(player.GetHealthController());
            manaBar.Init(player.GetManaController());
        }
    }
}