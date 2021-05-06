using Godot;

namespace BitRoguelike.Scripts.Items
{
    public class ItemStats : Resource
    {
        [Export] public string name = "Trash";
        [Export] public string description = "Some Description";
        [Export] public float weight = 0;
        [Export] public float value = 0;
        [Export] public Texture inventoryIcon;
        [Export] public Texture worldSprite;
    }
}