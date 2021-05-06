using Godot;

namespace BitRoguelike.Scripts.Map.Objects
{
    public class Lamp : Node2D
    {
        [Export] private float breathingSpeed = 0.5f;
        [Export] private float breathingSpeedRandom = 1.0f;

        [Export(PropertyHint.Range, "0, 5")] private float minEnergy = 0.5f;
        [Export(PropertyHint.Range, "1, 10")] private float maxEnergy = 1.5f;
    
    
        [Export] private NodePath lightPath;
        private Light2D light;

        private float lastFlicker = 0;
        private float currentEnergy = 0;
        private bool increasing = false;

        private static bool randomized = false;
    
        public override void _Ready()
        {
            if (!randomized)
            {
                GD.Randomize();
                randomized = true;
            }
            light = GetNode<Light2D>(lightPath);
            currentEnergy = (float) GD.RandRange(minEnergy, maxEnergy);
            light.Energy = currentEnergy;
        }

        public override void _Process(float delta)
        {
            if (increasing)
            {
                currentEnergy += delta * (breathingSpeed + (float) GD.RandRange(0, breathingSpeedRandom));
                if (currentEnergy >= maxEnergy)
                {
                    currentEnergy = maxEnergy;
                    increasing = false;
                }
            }
            else
            {
                currentEnergy -= delta * (breathingSpeed + (float) GD.RandRange(0, breathingSpeedRandom));
                if (currentEnergy <= minEnergy)
                {
                    currentEnergy = minEnergy;
                    increasing = true;
                }
            }

            light.Energy = currentEnergy;
        }
    }
}
