using GameboyRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Player
{
    public class Player : GameCharacter, IDefender, IAttacker
    {
        private HealthController healthController;
        private ManaController manaController;
        private PlayerMovementController movementController;
        private InventoryController inventoryController;

        public PlayerMovementController GetMovementController() => movementController;
        public HealthController GetHealthController() => healthController;
        public ManaController GetManaController() => manaController;

        public override void _Ready()
        {
            base._Ready();

            healthController = GetNode<HealthController>("HealthController");
            manaController = GetNode<ManaController>("ManaController");
            movementController = GetNode<PlayerMovementController>("MovementController");
            inventoryController = GetNode<InventoryController>("InventoryController");

            movementController.Init(
                this,
                GetAnimationPlayer(),
                GetRayCast2D(),
                GetTween()
            );
            inventoryController.Init(
                GetArmorController(),
                GetWeaponController()
            );

            healthController.Connect(nameof(HealthController.OnDeath), this, nameof(PlayerDied));
        }

        private void PlayerDied()
        {
            QueueFree();
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