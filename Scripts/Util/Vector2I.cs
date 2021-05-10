using Godot;

namespace BitRoguelike.Scripts.Util
{
    public struct Vector2I
    {
        public int x;
        public int y;

        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2I(Vector2 other) : this((int) other.x, (int) other.y)
        {
        }
    }
}