using System;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public enum ResourceChangeType
    {
        GAIN,
        LOSE
    }

    public struct ResourceChangeData
    {
        public ResourceChangeType changeType;
        public int newValue;
        public int maxValue;

        public ResourceChangeData(ResourceChangeType changeType, int newValue, int maxValue)
        {
            this.changeType = changeType;
            this.newValue = newValue;
            this.maxValue = maxValue;
        }
    }

    public abstract class ResourceController : Node
    {
        public Action<ResourceChangeData> OnResourceChange;

        public abstract int GetValue();
        public abstract float GetPercentValue();
    }
}