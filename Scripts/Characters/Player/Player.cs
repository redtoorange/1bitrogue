using System;
using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.UI;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class Player : GameCharacter, IDefender, IAttacker
    {
        // Actions
        public static Action OnPlayerActionStarted;
        public static Action OnPlayerActionCompleted;
        
        // Owned Controllers
        private HealthController healthController;
        private ManaController manaController;
        private InventoryController inventoryController;
        private GroundItemController groundItemController;
        private PlayerInputController playerInputController;

        // Injected Dependencies
        private PlayerUiController playerUiController;
        
        // Getters
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
        }

        public void Init(PlayerUiController playerUiController)
        {
            this.playerUiController = playerUiController;
            
            GetMovementController().OnMoveStarted += HandleActionStarted;
            GetMovementController().OnMoveCompleted += HandleActionFinished;
            healthController.OnDie += PlayerDied;

            playerInputController.Init(GetMovementController());
            inventoryController.Init(
                GetArmorController(),
                GetWeaponController(),
                groundItemController
            );
            groundItemController.Init(
                inventoryController
            );
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

        private void HandleActionStarted()
        {
            OnPlayerActionStarted?.Invoke();
        }
        
        private void HandleActionFinished()
        {
            OnPlayerActionCompleted?.Invoke();
        }
    }
}