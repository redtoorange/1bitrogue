using BitRoguelike.Scripts.Characters.Player;
using BitRoguelike.Scripts.UI;
using Godot;

namespace BitRoguelike.Scripts
{
    public class MainGame : Node
    {
        [Export] private NodePath playerPath = null;
        [Export] private NodePath playerUiControllerPath = null;
        [Export] private NodePath visionControllerPath = null;
        [Export] private NodePath mapControllerPath = null;

        private Player player;
        private PlayerUiController playerUiController;
        private VisionController visionController;
        private MapController mapController;
    
        public override void _Ready()
        {
            FetchDependencies();
            
            Init();
        }

        private void FetchDependencies()
        {
            player = GetNode<Player>(playerPath);
            playerUiController = GetNode<PlayerUiController>(playerUiControllerPath);
            visionController = GetNode<VisionController>(visionControllerPath);
            mapController = GetNode<MapController>(mapControllerPath);
        }
        
        private void Init()
        {
            player.Init(playerUiController, visionController);
            playerUiController.Init(player);
            visionController.Init(player, mapController);
        }
    }
}
