using Godot;

[Tool]
public class Test : EditorPlugin
{
    public override void _EnterTree()
    {
        AddCustomType("TestToolResource",
            "Resource",
            GD.Load<Script>("res://addons/TestAddon/TestToolResource.cs"),
            null
        );
    }

    public override void _ExitTree()
    {
        RemoveCustomType("TestToolResource");
    }
}