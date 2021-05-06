using BitRoguelike.Scripts.Characters.Player;
using BitRoguelike.Scripts.Managers;
using BitRoguelike.Scripts.Map.Objects;
using BitRoguelike.Scripts.UI.Inventory;
using BitRoguelike.Scripts.UI.Inventory.ContextMenu;
using Godot;

namespace BitRoguelike.Scripts.UI
{
    public enum PlayerUiState
    {
        GAME,
        INVENTORY,
        INVENTORY_CONTEXT_MENU,
        INVENTORY_CHEST,
        MENU,
        CHEST
    }

    public class PlayerUiController : Node
    {
        [Export] private NodePath playerHudPath;
        [Export] private NodePath playerInventoryUiPath;
        [Export] private NodePath pauseMenuPath;
        [Export] private NodePath contextMenuPath;

        private PlayerResourceDisplayController playerResourceDisplayController;
        private PlayerInventoryUiManager playerInventoryUiController;
        private PauseMenuController pauseMenu;
        private ContextMenuController contextMenu;

        private PlayerUiState currentState = PlayerUiState.GAME;

        public PlayerInventoryUiManager GetPlayerInventoryUiController() => playerInventoryUiController;

        public override void _Ready()
        {
            playerResourceDisplayController = GetNode<PlayerResourceDisplayController>(playerHudPath);
            playerInventoryUiController = GetNode<PlayerInventoryUiManager>(playerInventoryUiPath);
            pauseMenu = GetNode<PauseMenuController>(pauseMenuPath);
            contextMenu = GetNode<ContextMenuController>(contextMenuPath);

            pauseMenu.OnMenuClosed += HandleOnChestClosed;
        }

        public override void _EnterTree()
        {
            LootChest.OnChestOpened += HandleOnChestOpened;
        }

        public override void _ExitTree()
        {
            LootChest.OnChestOpened -= HandleOnChestOpened;
        }

        public void Init(Player player)
        {
            playerResourceDisplayController.Init(
                player.GetHealthController(),
                player.GetManaController()
            );
            playerInventoryUiController.Init(
                player.GetInventoryController(),
                contextMenu
            );
        }

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("Inventory"))
            {
                HandleInventory();
            }
            else if (@event.IsActionPressed("Back"))
            {
                HandleBack();
            }
        }

        private void HandleBack()
        {
            switch (currentState)
            {
                case PlayerUiState.GAME:
                    HandleOnMenuOpened();
                    break;
                case PlayerUiState.MENU:
                    HandleOnMenuClosed();
                    break;
                case PlayerUiState.INVENTORY:
                    HandleOnInventoryClosed();
                    break;
                case PlayerUiState.INVENTORY_CHEST:
                    HandleOnChestClosed();
                    break;
            }
        }

        private void HandleInventory()
        {
            switch (currentState)
            {
                case PlayerUiState.GAME:
                    HandleOnInventoryOpened();
                    break;
                case PlayerUiState.INVENTORY:
                    HandleOnInventoryClosed();
                    break;
                case PlayerUiState.INVENTORY_CHEST:
                    HandleOnChestClosed();
                    HandleOnInventoryOpened();
                    break;
            }
        }

        private void HandleOnMenuOpened()
        {
            currentState = PlayerUiState.MENU;
            pauseMenu.Show();
            GameRoundManager.S.SetGamePaused(true);
        }

        private void HandleOnMenuClosed()
        {
            currentState = PlayerUiState.GAME;
            pauseMenu.Hide();
            GameRoundManager.S.SetGamePaused(false);
        }

        private void HandleOnInventoryClosed()
        {
            currentState = PlayerUiState.GAME;
            playerInventoryUiController.Hide();
            GameRoundManager.S.SetGamePaused(false);
        }
        private void HandleOnInventoryOpened()
        {
            currentState = PlayerUiState.INVENTORY;
            playerInventoryUiController.Show();
            GameRoundManager.S.SetGamePaused(true);
        }
        
        private void HandleOnChestOpened(LootChest chest)
        {
            GameRoundManager.S.SetGamePaused(true);
            currentState = PlayerUiState.INVENTORY_CHEST;
            
            playerInventoryUiController.OpenChest(chest);
            playerInventoryUiController.Show();
        }
        
        private void HandleOnChestClosed()
        {
            playerInventoryUiController.Hide();
            playerInventoryUiController.CloseChest();
            
            currentState = PlayerUiState.GAME;
            GameRoundManager.S.SetGamePaused(false);
        }
    }
}