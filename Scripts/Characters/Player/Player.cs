using System;
using GameboyRoguelike.Scripts.Characters.Controllers;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class Player : GameCharacter, IDefender, IAttacker
    {
        public static Action OnPlayerTurnFinished;
        
        private HealthController healthController;
        private ManaController manaController;
        private InventoryController inventoryController;
        private GroundItemController groundItemController;
        
        private PlayerInputController playerInputController;

        public HealthController GetHealthController() => healthController;
        public ManaController GetManaController() => manaController;
        public GroundItemController GetGroundItemController() => groundItemController;

        public override void _Ready()
        {
            base._Ready();

            healthController = GetNode<HealthController>("Controllers/HealthController");
            manaController = GetNode<ManaController>("Controllers/ManaController");
            inventoryController = GetNode<InventoryController>("Controllers/InventoryController");
            groundItemController = GetNode<GroundItemController>("Controllers/GroundItemController");
            
            playerInputController = GetInputController() as PlayerInputController;
            
            GetMovementController().OnCompletedMove += HandlePerformedAction;
            
            playerInputController.Init(GetMovementController());
            inventoryController.Init(
                GetArmorController(),
                GetWeaponController(),
                groundItemController
            );
            groundItemController.Init(
                inventoryController
            );
            
            healthController.OnDie += PlayerDied;
        }

        private void PlayerDied(HealthController healthController)
        {
            if (this.healthController == healthController)
            {
                QueueFree();
            }
        }

        public int GetArmorClass()
        {
            return 10;
        }

        public void TakeDamage(int amount)
        {
            healthController.TakeDamage(amount);
        }

        public int GetHitBonus()
        {
            return 0;
        }

        public int GetWeapon()
        {
            return 5;
        }

        public int GetDamageBonus()
        {
            return 0;
        }

        private void HandlePerformedAction()
        {
            OnPlayerTurnFinished?.Invoke();
        }
    }
}