using Godot;
using System;
using GameboyRoguelike.Scripts;
using GameboyRoguelike.Scripts.UI.Inventory;

public class Main : Node
{
    [Export] private NodePath mainMenuPath;
    [Export] private PackedScene mainGameScene;

    private MainMenu mainMenu;
    private MainGame mainGame;
    
    public override void _Ready()
    {
        mainMenu = GetNode<MainMenu>(mainMenuPath);
        mainMenu.OnStartClicked += HandleOnStartPressed;

        PauseMenuController.OnQuitToMainMenu += HandleOnQuitToMainMenu;
    }

    private void HandleOnStartPressed()
    {
        mainMenu.Visible = false;
        
        mainGame = mainGameScene.Instance<MainGame>();
        AddChild(mainGame);
    }

    private void HandleOnQuitToMainMenu()
    {
        RemoveChild(mainGame);
        mainGame.QueueFree();
        
        mainMenu.Visible = true;
    }
}
