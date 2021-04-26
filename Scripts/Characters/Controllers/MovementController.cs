using System;
using GameboyRoguelike.Scripts.Map.Objects;
using Godot;
using Object = Godot.Object;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class MovementController : Node
    {
        public Action OnCompletedMove;

        [Export] private float movementSpeed = 0.2f;
        private GameCharacter gameCharacter = null;
        protected AnimationPlayer animationPlayer = null;
        protected Tween tween = null;
        protected RayCast2D rayCast2D = null;
        protected bool isTweening = false;

        public void Init(GameCharacter gameCharacter, AnimationPlayer animationPlayer, RayCast2D rayCast2D, Tween tween)
        {
            this.animationPlayer = animationPlayer;
            this.rayCast2D = rayCast2D;
            this.tween = tween;
            this.gameCharacter = gameCharacter;

            this.animationPlayer.Connect("animation_completed", this, nameof(AnimationCompleted));
        }

        private void AnimationCompleted(string name)
        {
            isTweening = false;
            OnCompletedMove?.Invoke();
        }

        public void MoveTo(Vector2 destination)
        {
            if (!tween.IsActive() && CanMoveTo(destination))
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
        }

        private bool CanMoveTo(Vector2 destination)
        {
            bool pathClear = true;

            rayCast2D.CastTo = destination;
            rayCast2D.ForceRaycastUpdate();

            if (rayCast2D.IsColliding())
            {
                pathClear = false;

                Node collider = rayCast2D.GetCollider() as Node;
                GD.Print("Collider: " + collider.Name);
                if (collider is IInteractable interactable)
                {
                    interactable.Interact();
                }
                else if (collider is IDefender target && gameCharacter is IAttacker attacker)
                {
                    PlayLungeAnimation(destination);
                    AttackSystem.S.ResolveAttack(attacker, target);
                }
            }

            return pathClear;
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
    }
}