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

        private PlayerResourceDisplayController playerResourceDisplayController;
        private PlayerInventoryUiManager playerInventoryUiUi;
        private PauseMenuController pauseMenu;

        private PlayerUiState currentState = PlayerUiState.GAME;

        public override void _Ready()
        {
            playerResourceDisplayController = GetNode<PlayerResourceDisplayController>(playerHudPath);
            playerInventoryUiUi = GetNode<PlayerInventoryUiManager>(playerInventoryUiPath);
            pauseMenu = GetNode<PauseMenuController>(pauseMenuPath);

            pauseMenu.OnMenuClosed += HandleOnMenuClicked;
        }

        public void Init(Player player)
        {
            playerResourceDisplayController.Init(player);
            playerInventoryUiUi.Init(player);
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("Inventory"))
            {
                switch (currentState)
                {
                    case PlayerUiState.GAME:
                        currentState = PlayerUiState.INVENTORY;
                        playerInventoryUiUi.Show();
                        break;
                    case PlayerUiState.INVENTORY:
                        currentState = PlayerUiState.GAME;
                        playerInventoryUiUi.Hide();
                        break;
                }
            }
            else if (Input.IsActionJustPressed("Back"))
            {
                switch (currentState)
                {
                    case PlayerUiState.GAME:
                        currentState = PlayerUiState.MENU;
                        pauseMenu.Show();
                        break;
                    case PlayerUiState.MENU:
                        currentState = PlayerUiState.GAME;
                        pauseMenu.Hide();
                        break;
                    case PlayerUiState.INVENTORY:
                        currentState = PlayerUiState.GAME;
                        playerInventoryUiUi.Hide();
                        break;
                }
            }
        }

        private void HandleOnMenuClicked()
        {
            currentState = PlayerUiState.GAME;
            pauseMenu.Hide();
        }
    }
}