using GameboyRoguelike.Scripts.Characters.Player;
using GameboyRoguelike.Scripts.UI.Inventory;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public enum PlayerUiState
    {
        GAME,
        INVENTORY,
        MENU,
        CHEST
    }

    public class PlayerUiController : Node
    {
        [Export] private NodePath playerHudPath;
        [Export] private NodePath playerInventoryUiPath;
        [Export] private NodePath pauseMenuPath;

        private PlayerHUDController playerHudController;
        private PlayerInventoryManager playerInventoryUi;
        private PauseMenuController pauseMenu;

        private PlayerUiState currentState = PlayerUiState.GAME;

        public override void _Ready()
        {
            playerHudController = GetNode<PlayerHUDController>(playerHudPath);
            playerInventoryUi = GetNode<PlayerInventoryManager>(playerInventoryUiPath);
            pauseMenu = GetNode<PauseMenuController>(pauseMenuPath);
        }

        public void Init(Player player)
        {
            playerHudController.Init(player);
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("Inventory"))
            {
                playerInventoryUi.Show();
            }
        }
    }
}