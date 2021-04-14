using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class MainUI : Node
    {
        [Export] private NodePath playerUiPath;

        private PlayerUI playerUi;

        public override void _Ready()
        {
            playerUi = GetNode<PlayerUI>(playerUiPath);
        }

        public void Init(Player player)
        {
            playerUi.Init(player);
        }
    }
}