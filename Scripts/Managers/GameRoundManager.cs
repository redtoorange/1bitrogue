using System.Collections.Generic;
using GameboyRoguelike.Scripts.Characters;
using GameboyRoguelike.Scripts.Characters.Player;
using Godot;

namespace GameboyRoguelike.Scripts.Managers
{
    public enum GameTimingState
    {
        REALTIME,
        TURN_BASED,
    }

    public enum GameTurnState
    {
        WAITING_ON_PLAYER,
        TICKING_ENTITIES
    }
    
    public class GameRoundManager : Node
    {
        public static GameRoundManager S;

        [Export] private GameTimingState currentTimingState = GameTimingState.REALTIME;
        [Export] private float playerTurnCooldown = 0.25f;
        
        private GameTurnState currentGameTurnState = GameTurnState.WAITING_ON_PLAYER;
        private float coolDownTimer = 0.0f;
        private bool onCooldown = false;
        private List<ITurnTaker> turnTakers = new List<ITurnTaker>();
        private Queue<ITurnTaker> tickingEntities;

        public override void _EnterTree()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                GD.PrintErr("More than one GameRoundManager exists in tree");
                QueueFree();
            }

            Player.OnPlayerTurnFinished += HandleOnPlayerTurnFinished;
        }

        public override void _ExitTree()
        {
            Player.OnPlayerTurnFinished -= HandleOnPlayerTurnFinished;
        }

        private void HandleOnPlayerTurnFinished()
        {
            if (currentTimingState == GameTimingState.TURN_BASED)
            {
                coolDownTimer = playerTurnCooldown;
                onCooldown = true;
                
                InitializeTickingEntities();
                currentGameTurnState = GameTurnState.TICKING_ENTITIES;
            }
        }

        /// <summary>
        /// Can the player currently move or is he waiting for the world to finish ticking
        /// </summary>
        public bool CanPlayerAct()
        {
            if (onCooldown) return false;
            
            if (currentTimingState == GameTimingState.REALTIME)
            {
                return true;
            }

            return currentGameTurnState == GameTurnState.WAITING_ON_PLAYER;
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
            
            if (currentGameTurnState == GameTurnState.TICKING_ENTITIES)
            {
                ProcessTickingEntities(delta);

                if (tickingEntities.Count == 0)
                {
                    currentGameTurnState = GameTurnState.WAITING_ON_PLAYER;
                }
            }
        }
        
        /// <summary>
        /// Initialize all registered TurnTakers to begin ticking.
        /// </summary>
        private void InitializeTickingEntities()
        {
            tickingEntities = new Queue<ITurnTaker>(turnTakers.Count);
            
            for (int i = 0; i < turnTakers.Count; i++)
            {
                ITurnTaker turnTaker = turnTakers[i];
                turnTaker.SetTickState(TurnTakerState.WAITING);
                tickingEntities.Enqueue(turnTaker);
            }
        }

        /// <summary>
        /// Process all the currently registered TurnTakers.
        /// </summary>
        private void ProcessTickingEntities(float delta)
        {
            Queue<ITurnTaker> tickedEntities = new Queue<ITurnTaker>();

            while (tickingEntities.Count > 0)
            {
                ITurnTaker turnTaker = tickingEntities.Dequeue();
                if (turnTaker.GetTickState() == TurnTakerState.WAITING)
                {
                    turnTaker.InitTick();
                    turnTaker.SetTickState(TurnTakerState.TICKING);
                    tickedEntities.Enqueue(turnTaker);
                }
                else if (turnTaker.GetTickState() == TurnTakerState.TICKING)
                {
                    turnTaker.Tick(delta);
                    tickedEntities.Enqueue(turnTaker);
                }
            }

            tickingEntities = tickedEntities;
        }

        public void RegisterTurnTaker(ITurnTaker turnTaker)
        {
            if (!turnTakers.Contains(turnTaker))
            {
                turnTakers.Add(turnTaker);
            }
        }

        public void UnregisterTurnTaker(ITurnTaker turnTaker)
        {
            if (turnTakers.Contains(turnTaker))
            {
                turnTakers.Remove(turnTaker);

                if (tickingEntities.Contains(turnTaker))
                {
                    RemoveEntity(turnTaker);
                }
            }
        }

        /// <summary>
        /// If the removed entity (probably died, is currently ticking, we need to remove it to avoid holding a reference
        /// to a freed entity
        /// </summary>
        private void RemoveEntity(ITurnTaker turnTaker)
        {
            Queue<ITurnTaker> tempQueue = new Queue<ITurnTaker>(tickingEntities.Count - 1);
            while (tickingEntities.Count > 0)
            {
                ITurnTaker tempEntity = tickingEntities.Dequeue();
                if (tempEntity != turnTaker)
                {
                    tempQueue.Enqueue(tempEntity);
                }
            }

            tickingEntities = tempQueue;
        }
    }
}