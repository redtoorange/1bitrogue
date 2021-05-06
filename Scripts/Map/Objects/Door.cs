using Godot;

namespace BitRoguelike.Scripts.Map.Objects
{
    public enum DoorState
    {
        OPENED,
        CLOSED,
        LOCKED
    }

    public class Door : StaticBody2D, IInteractable
    {
        [Export] private DoorState currentState = DoorState.CLOSED;

        [Export] private NodePath animationPlayerPath = null;
        [Export] private NodePath lightOccluder2DPath = null;
        [Export] private NodePath collisionShape2DPath = null;
        [Export] private NodePath openSpritePath = null;
        [Export] private NodePath closedSpritePath = null;

        private AnimationPlayer animationPlayer = null;
        private LightOccluder2D lightOccluder2D = null;
        private CollisionShape2D collisionShape2D = null;
        private Sprite openSprite = null;
        private Sprite closedSprite = null;

        public override void _Ready()
        {
            animationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);
            lightOccluder2D = GetNode<LightOccluder2D>(lightOccluder2DPath);
            collisionShape2D = GetNode<CollisionShape2D>(collisionShape2DPath);
            openSprite = GetNode<Sprite>(openSpritePath);
            closedSprite = GetNode<Sprite>(closedSpritePath);
        }

        public void OpenDoor()
        {
            currentState = DoorState.OPENED;
            collisionShape2D.Disabled = true;
            openSprite.Visible = true;
            closedSprite.Visible = false;
            lightOccluder2D.Visible = false;
        }

        public void CloseDoor()
        {
            currentState = DoorState.CLOSED;
            collisionShape2D.Disabled = false;
            openSprite.Visible = false;
            closedSprite.Visible = true;
            lightOccluder2D.Visible = true;
        }

        public void Interact()
        {
            if (currentState == DoorState.CLOSED)
            {
                OpenDoor();
            }
            else if (currentState == DoorState.LOCKED)
            {
                animationPlayer.Play("DoorText");
            }
        }
    }
}