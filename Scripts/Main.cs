using BitRoguelike.Scripts.UI;
using BitRoguelike.Scripts.UI.MainMenu;
using Godot;

namespace BitRoguelike.Scripts
{
    public class Main : Node
    {
        [Export] private NodePath mainMenuPath = null;
        [Export] private NodePath settingsMenuPath = null;
        [Export] private PackedScene mainGameScene = null;

        private MainMenu mainMenu;
        private SettingsMenuController settingsMenuController;
        private MainGame mainGame;
    
        public override void _Ready()
        {
            mainMenu = GetNode<MainMenu>(mainMenuPath);
            settingsMenuController = GetNode<SettingsMenuController>(settingsMenuPath);

            mainMenu.OnStartClicked += HandleOnStartPressed;
            mainMenu.OnSettingsClicked += HandleOnSettingsPressed;

            settingsMenuController.OnSettingsMenuBackPressed += HandleOnSettingsBackPressed;
        
            PauseMenuController.OnQuitToMainMenu += HandleOnQuitToMainMenu;
        }

        private void HandleOnStartPressed()
        {
            mainMenu.Visible = false;
        
            mainGame = mainGameScene.Instance<MainGame>();
            AddChild(mainGame);
        }
    
        private void HandleOnSettingsPressed()
        {
            settingsMenuController.Visible = true;
        }

        private void HandleOnSettingsBackPressed()
        {
            settingsMenuController.Visible = false;
        }

        private void HandleOnQuitToMainMenu()
        {
            RemoveChild(mainGame);
            mainGame.QueueFree();
        
            mainMenu.Visible = true;
        }
    }
}
