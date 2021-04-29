using GameboyRoguelike.Scripts.Characters.Player;
using GameboyRoguelike.Scripts.UI;
using Godot;

namespace GameboyRoguelike.Scripts
{
    public class MainGame : Node
    {
        [Export] private NodePath playerPath;
        [Export] private NodePath mainHUDPath;

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
            playerUiController = GetNode<PlayerUiController>(mainHUDPath);
        }
        
        private void Init()
        {
            player.Init(playerUiController);
            playerUiController.Init(player);
        }
    }
}
