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
        private VisionController visionController;
        
        
        public void Init(MovementController movementController, VisionController visionController)
        {
            this.movementController = movementController;
            this.visionController = visionController;
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
            bool moved = false;
            
            if (Input.IsActionPressed(MOVE_UP))
            {
                moved = movementController.AttemptMove(moveUpVector);
            }
            else if (Input.IsActionPressed(MOVE_DOWN))
            {
                moved = movementController.AttemptMove(moveDownVector);
            }
            else if (Input.IsActionPressed(MOVE_LEFT))
            {
                moved = movementController.AttemptMove(moveLeftVector);
            }
            else if (Input.IsActionPressed(MOVE_RIGHT))
            {
                moved = movementController.AttemptMove(moveRightVector);
            }

            if (moved)
            {
                visionController.UpdateFov(movementController.GetDestination());
            }
            
        }
    }
}