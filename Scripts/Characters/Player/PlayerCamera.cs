using BitRoguelike.Scripts.Characters.Player;
using Godot;

public class PlayerCamera : Camera2D
{
    [Export] private NodePath playerPath;
    private Player player;

    public override void _Ready()
    {
        player = GetNode<Player>(playerPath);
        SmoothingEnabled = false;
        Position = player.Position;
        SmoothingEnabled = true;
    }

    public override void _Process(float delta)
    {
        Position = player.Position;
    }
}