using System;
using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Managers;
using GameboyRoguelike.Scripts.Map.Objects;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class PlayerMovementController : MovementController
    {
        public Action OnPlayerCompletedAction;

        [Export] private float movementCooldown = 0.2f;
        private float coolDownTimer = 0.0f;

        [Export] private string MOVE_UP = "MoveUp";
        private Vector2 moveUpVector = new Vector2(0, -16);

        [Export] private string MOVE_DOWN = "MoveDown";
        private Vector2 moveDownVector = new Vector2(0, 16);

        [Export] private string MOVE_LEFT = "MoveLeft";
        private Vector2 moveLeftVector = new Vector2(-16, 0);

        [Export] private string MOVE_RIGHT = "MoveRight";
        private Vector2 moveRightVector = new Vector2(16, 0);

        private Player player = null;
        private bool onCooldown = false;

        public void Init(Player player, AnimationPlayer animationPlayer, RayCast2D rayCast2D, Tween tween)
        {
            this.player = player;
            base.Init(animationPlayer, rayCast2D, tween);

            animationPlayer.Connect("animation_finished", this, nameof(AnimationFinished));
        }

        public override void _Process(float delta)
        {
            
            if (onCooldown && coolDownTimer > 0)
            {
                coolDownTimer -= delta;
                if (coolDownTimer <= 0)
                {
                    onCooldown = false;
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (!GameRoundManager.S.CanPlayerAct() || isTweening || onCooldown) return;

            if (Input.IsActionPressed(MOVE_UP))
            {
                MoveTo(moveUpVector);
            }
            else if (Input.IsActionPressed(MOVE_DOWN))
            {
                MoveTo(moveDownVector);
            }
            else if (Input.IsActionPressed(MOVE_LEFT))
            {
                MoveTo(moveLeftVector);
            }
            else if (Input.IsActionPressed(MOVE_RIGHT))
            {
                MoveTo(moveRightVector);
            }
        }

        private void MoveTo(Vector2 destination)
        {
            if (!tween.IsActive() && CanMoveTo(destination))
            {
                tween.InterpolateProperty(player,
                    "position",
                    player.Position,
                    player.Position + destination,
                    movementCooldown,
                    Tween.TransitionType.Cubic);
                animationPlayer.Play("Hop");
                tween.Start();
                isTweening = true;
                SetOnCooldown();
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
                    // PlayLungeAnimation(destination);
                    interactable.Interact();
                    SetOnCooldown();
                }
                else if (collider is IDefender target)
                {
                    PlayLungeAnimation(destination);
                    AttackSystem.S.ResolveAttack(player, target);
                    SetOnCooldown();
                }
            }

            return pathClear;
        }

        private void SetOnCooldown()
        {
            onCooldown = true;
            coolDownTimer = movementCooldown;
        }

        private void PlayLungeAnimation(Vector2 destination)
        {
            // RIGHT
            if (destination.x > 0)
            {
                animationPlayer.Play("LungeRight");
            }

            // LEFT
            if (destination.x < 0)
            {
                animationPlayer.Play("LungeLeft");
            }

            // DOWN
            if (destination.y > 0)
            {
                animationPlayer.Play("LungeDown");
            }

            // UP
            if (destination.y < 0)
            {
                animationPlayer.Play("LungeUp");
            }
        }

        private void AnimationFinished(string animName)
        {
            OnPlayerCompletedAction?.Invoke();
        }
    }
}