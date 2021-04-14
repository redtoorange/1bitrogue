using Godot;

namespace GameboyRoguelike.Scripts.Map.Objects
{
    public enum DoorState
    {
        OPENED,
        CLOSED,
        LOCKED
    }

    public class Door : StaticBody2D, IInteractable
    {
        [Export]
        private DoorState currentState = DoorState.CLOSED;
        [Export]
        private NodePath animationPlayerPath = null;
        private AnimationPlayer animationPlayer = null;

        private CollisionShape2D collisionShape2D = null;
        private Sprite openSprite = null;
        private Sprite closedSprite = null;

        public override void _Ready()
        {
            collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

            openSprite = GetNode<Sprite>("OpenSprite");
            closedSprite = GetNode<Sprite>("ClosedSprite");

            animationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);
        }

        public void OpenDoor()
        {
            currentState = DoorState.OPENED;
            collisionShape2D.Disabled = true;
            openSprite.Visible = true;
            closedSprite.Visible = false;
        }

        public void CloseDoor()
        {
            currentState = DoorState.CLOSED;
            collisionShape2D.Disabled = false;
            openSprite.Visible = false;
            closedSprite.Visible = true;
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