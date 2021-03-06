using Godot;

namespace BitRoguelike.Scripts.Characters.NPC
{
    [Tool]
    public class NPCCharacter : GameCharacter
    {
        [Export] private Texture characterSprite = null;
        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                base._Ready();
            }

            if (characterSprite == null)
            {
                GD.PrintErr("Null characterSprite");
            }
            
            Sprite s = GetNode<Sprite>("Sprite");
            if (s == null)
            {
                GD.PrintErr("Null Sprite Node");
            }
            s.Texture = characterSprite;
            Update();
        }
    }
}