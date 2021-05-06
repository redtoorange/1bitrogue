using BitRoguelike.Scripts.Characters.Player;
using BitRoguelike.Scripts.UI;
using Godot;

namespace BitRoguelike.Scripts
{
    public class MainGame : Node
    {
        [Export] private NodePath playerPath;
        [Export] private NodePath playerUIController;

        private Player player;
        private PlayerUiController playerUiController;
    
        public override void _Ready()
        {
            FetchDependencies();
            
            Init();
        }

        private void FetchDependencies()
        {
            player = GetNode<Player>(playerPath);
            playerUiController = GetNode<PlayerUiController>(playerUIController);
        }
        
        private void Init()
        {
            player.Init(playerUiController);
            playerUiController.Init(player);
        }
    }
}
