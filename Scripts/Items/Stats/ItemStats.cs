using Godot;

namespace GameboyRoguelike.Scripts.Items.Stats
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