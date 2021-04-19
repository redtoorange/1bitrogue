using GameboyRoguelike.Scripts.Characters.Controllers;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class Player : GameCharacter, IDefender, IAttacker
    {
        private HealthController healthController;
        private ManaController manaController;
        private PlayerMovementController movementController;
        private InventoryController inventoryController;
        private GroundItemController groundItemController;

        public PlayerMovementController GetMovementController() => movementController;
        public HealthController GetHealthController() => healthController;
        public ManaController GetManaController() => manaController;
        public GroundItemController GetGroundItemController() => groundItemController;

        public override void _Ready()
        {
            base._Ready();

            healthController = GetNode<HealthController>("HealthController");
            manaController = GetNode<ManaController>("ManaController");
            movementController = GetNode<PlayerMovementController>("MovementController");
            inventoryController = GetNode<InventoryController>("InventoryController");
            groundItemController = GetNode<GroundItemController>("GroundItemController");

            movementController.Init(
                this,
                GetAnimationPlayer(),
                GetRayCast2D(),
                GetTween()
            );
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
    }
}