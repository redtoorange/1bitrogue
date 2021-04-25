namespace GameboyRoguelike.Scripts.Characters
{
    public enum TurnTakerState
    {
        WAITING,
        TICKING,
        DONE
    }
    
    public interface ITurnTaker
    {
        void InitTick();
        void Tick(float deltaTime);
        
        TurnTakerState GetTickState();
        void SetTickState(TurnTakerState newState);
    }
}