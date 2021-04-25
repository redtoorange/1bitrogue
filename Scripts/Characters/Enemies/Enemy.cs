using GameboyRoguelike.Scripts.Characters.Controllers;
using GameboyRoguelike.Scripts.Managers;
using GameboyRoguelike.Scripts.UI;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Enemies
{
    public class Enemy : GameCharacter, IDefender, IAttacker, ITurnTaker
    {
        [Export] private NodePath healthControllerPath;
        [Export] private NodePath movementControllerPath;
        [Export] private NodePath healthBarPath;

        private HealthController healthController;
        private MovementController movementController;
        private ResourceBar healthBar;

        public MovementController GetMovementController() => movementController;
        public HealthController GetHealthController() => healthController;

        public override void _Ready()
        {
            base._Ready();

            healthController = GetNode<HealthController>(healthControllerPath);
            movementController = GetNode<MovementController>(movementControllerPath);
            healthBar = GetNode<ResourceBar>(healthBarPath);

            movementController.Init(
                GetAnimationPlayer(),
                GetRayCast2D(),
                GetTween()
            );

            healthBar.Init(healthController);
            healthController.OnDie += Died;

            GameRoundManager.S.RegisterTurnTaker(this);
        }

        public override void _ExitTree()
        {
            GameRoundManager.S.UnregisterTurnTaker(this);
        }

        private void Died(HealthController healthController)
        {
            if (healthController == this.healthController)
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
            return 1;
        }

        public int GetDamageBonus()
        {
            return 0;
        }


        private TurnTakerState currentState = TurnTakerState.WAITING;
        public void InitTick()
        {
            GD.Print(Name + " is Initializing to ticking.");
        }

        public void Tick(float deltaTime)
        {
            GD.Print(Name + " is ticking.");
            healthController.HealDamage(1);
            currentState = TurnTakerState.DONE;
        }

        public TurnTakerState GetTickState()
        {
            return currentState;
        }

        public void SetTickState(TurnTakerState newState)
        {
            currentState = newState;
        }
    }
}