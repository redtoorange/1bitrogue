using Godot;
using System;

public class MainMenu : Control
{
    public Action OnStartClicked;
    
    [Export] private NodePath startButtonPath;
    [Export] private NodePath settingsButtonPath;
    [Export] private NodePath quitButtonPath;

    private Button startButton;
    private Button settingsButton;
    private Button quitButton;
    
    public override void _Ready()
    {
        startButton = GetNode<Button>(startButtonPath);
        settingsButton = GetNode<Button>(settingsButtonPath);
        quitButton = GetNode<Button>(quitButtonPath);

        startButton.Connect("pressed", this, nameof(OnStartPressed));
        settingsButton.Connect("pressed", this, nameof(OnSettingsPressed));
        quitButton.Connect("pressed", this, nameof(OnQuitPressed));
    }

    private void OnStartPressed()
    {
        OnStartClicked?.Invoke();
    }

    private void OnSettingsPressed()
    {
        
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
