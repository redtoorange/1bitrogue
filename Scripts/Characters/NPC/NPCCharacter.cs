using Godot;

namespace GameboyRoguelike.Scripts.Characters.NPC
{
    public class NPCCharacter : GameCharacter
    {
        [Export] private Texture characterSprite;
        [Export] private NodePath spritePath;
        public override void _Ready()
        {
            base._Ready();

            if (characterSprite == null)
            {
                GD.PrintErr("Null characterSprite");
            }
            
            Sprite s = GetNode<Sprite>(spritePath);
            if (s == null)
            {
                GD.PrintErr("Null Sprite Node");
            }
            s.Texture = characterSprite;
            Update();
        }
    }
}