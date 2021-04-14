using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.UI;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Enemies
{
    public class Enemy : GameCharacter, IDefender, IAttacker
    {
        [Export] private NodePath healthControllerPath;
        [Export] private NodePath movementControllerPath;
        [Export] private NodePath healthBarPath;

        private HealthController healthController;
        private MovementController movementController;
        private HealthBar healthBar;
    
        public MovementController GetMovementController() => movementController;
        public HealthController GetHealthController() => healthController;

        public override void _Ready()
        {
            base._Ready();

            healthController = GetNode<HealthController>(healthControllerPath);
            movementController = GetNode<MovementController>(movementControllerPath);
            healthBar = GetNode<HealthBar>(healthBarPath);

            movementController.Init(
                GetAnimationPlayer(),
                GetRayCast2D(),
                GetTween()
            );

            healthController.Connect(nameof(HealthController.OnDeath), this, nameof(Died));
        
            healthBar.Init(healthController);
        }

        private void Died()
        {
            GD.Print("Monster Died!");
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
            return 1;
        }

        public int GetDamageBonus()
        {
            return 0;
        }
    }
}