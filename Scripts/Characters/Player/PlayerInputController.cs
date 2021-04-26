using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Managers;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class PlayerInputController : InputController
    {
        [Export] private string MOVE_UP = "MoveUp";
        private Vector2 moveUpVector = new Vector2(0, -16);

        [Export] private string MOVE_DOWN = "MoveDown";
        private Vector2 moveDownVector = new Vector2(0, 16);

        [Export] private string MOVE_LEFT = "MoveLeft";
        private Vector2 moveLeftVector = new Vector2(-16, 0);

        [Export] private string MOVE_RIGHT = "MoveRight";
        private Vector2 moveRightVector = new Vector2(16, 0);

        private MovementController movementController;
        
        
        public void Init(MovementController movementController)
        {
            this.movementController = movementController;
        }
        
        public override void _Process(float delta)
        {
            if (!GameRoundManager.S.CanPlayerAct())
            {
                return;
            }

            if (Input.IsActionPressed(MOVE_UP))
            {
                movementController.MoveTo(moveUpVector);
            }
            else if (Input.IsActionPressed(MOVE_DOWN))
            {
                movementController.MoveTo(moveDownVector);
            }
            else if (Input.IsActionPressed(MOVE_LEFT))
            {
                movementController.MoveTo(moveLeftVector);
            }
            else if (Input.IsActionPressed(MOVE_RIGHT))
            {
                movementController.MoveTo(moveRightVector);
            }
        }
    }
}