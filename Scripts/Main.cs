using Godot;
using System;
using BitRoguelike.Scripts.UI;
using GameboyRoguelike.Scripts;
using GameboyRoguelike.Scripts.UI.Inventory;

public class Main : Node
{
    [Export] private NodePath mainMenuPath;
    [Export] private NodePath settingsMenuPath;
    [Export] private PackedScene mainGameScene;

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
