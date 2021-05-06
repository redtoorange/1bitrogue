using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public class TrinketController : Node
    {
        [Export] private Trinket leftRing = null;
        [Export] private Trinket rightRing = null;
        [Export] private Trinket necklaceRing = null;
    }
}