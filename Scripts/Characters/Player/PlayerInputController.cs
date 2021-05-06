using BitRoguelike.Scripts.Characters.Controllers;
using BitRoguelike.Scripts.Managers;
using Godot;

namespace BitRoguelike.Scripts.Characters.Player
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
            if (GameRoundManager.S.CanPlayerAct())
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            if (Input.IsActionPressed(MOVE_UP))
            {
                movementController.AttemptMove(moveUpVector);
            }
            else if (Input.IsActionPressed(MOVE_DOWN))
            {
                movementController.AttemptMove(moveDownVector);
            }
            else if (Input.IsActionPressed(MOVE_LEFT))
            {
                movementController.AttemptMove(moveLeftVector);
            }
            else if (Input.IsActionPressed(MOVE_RIGHT))
            {
                movementController.AttemptMove(moveRightVector);
            }
        }
    }
}