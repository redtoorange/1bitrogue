using System;
using BitRoguelike.Scripts.Map.Objects;
using BitRoguelike.Scripts.Systems;
using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public class MovementController : Node
    {
        public Action OnMoveCompleted;
        public Action OnMoveStarted;

        [Export] private float movementSpeed = 0.2f;
        private GameCharacter gameCharacter = null;
        protected AnimationPlayer animationPlayer = null;
        protected Tween tween = null;
        protected RayCast2D rayCast2D = null;
        
        protected bool isTweening = false;
        protected Timer timer = null;

        public override void _Ready()
        {
            timer = new Timer();
            AddChild(timer);
        }

        public void Init(GameCharacter gameCharacter, AnimationPlayer animationPlayer, RayCast2D rayCast2D, Tween tween)
        {
            this.animationPlayer = animationPlayer;
            this.rayCast2D = rayCast2D;
            this.tween = tween;
            this.gameCharacter = gameCharacter;

            this.animationPlayer.Connect("animation_finished", this, nameof(AnimationCompleted));
        }

        private void AnimationCompleted(string name)
        {
            isTweening = false;
        }

        public void AttemptMove(Vector2 destination)
        {
            if (tween.IsActive()) return;
            
            Node blocker = CanMoveTo(destination);
            if (blocker == null)
            {
                tween.InterpolateProperty(gameCharacter,
                    "position",
                    gameCharacter.Position,
                    gameCharacter.Position + destination,
                    movementSpeed,
                    Tween.TransitionType.Cubic);
                animationPlayer.Play("Hop");
                tween.Start();
                isTweening = true;
            }
            else
            {
                if (blocker is IInteractable interactable)
                {
                    interactable.Interact();
                }
                else if (blocker is IDefender target && gameCharacter is IAttacker attacker)
                {
                    AttackSystem.S.ResolveAttack(attacker, target);
                }

                PlayLungeAnimation(destination);
            }
            
            Timeout(1);
        }

        private Node CanMoveTo(Vector2 destination)
        {
            rayCast2D.CastTo = destination;
            rayCast2D.ForceRaycastUpdate();

            if (rayCast2D.IsColliding())
            {
                return rayCast2D.GetCollider() as Node;
            }

            return null;
        }

        private void PlayLungeAnimation(Vector2 destination)
        {
            if (destination.x > 0)
            {
                animationPlayer.Play("LungeRight");
            }
            else if (destination.x < 0)
            {
                animationPlayer.Play("LungeLeft");
            }
            else if (destination.y > 0)
            {
                animationPlayer.Play("LungeDown");
            }
            else if (destination.y < 0)
            {
                animationPlayer.Play("LungeUp");
            }
        }

        private async void Timeout(float amount)
        {
            OnMoveStarted?.Invoke();

            timer.Start(0.25f);
            await ToSignal(timer, "timeout");
            GD.Print("Timer finished");

            OnMoveCompleted?.Invoke();
        }
    }
}