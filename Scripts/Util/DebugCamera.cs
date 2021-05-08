using Godot;

public class DebugCamera : Node2D
{
    [Export] private float speed = 5.0f;
    
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("MoveUp"))
        {
            Position += new Vector2(0, -speed);
        }
        else if (Input.IsActionPressed("MoveDown"))
        {
            Position += new Vector2(0, speed);
        }
        else if (Input.IsActionPressed("MoveLeft"))
        {
            Position += new Vector2(-speed, 0);
        }
        else if (Input.IsActionPressed("MoveRight"))
        {
            Position += new Vector2(speed, 0);
        }
    }
}