using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public enum StatType
    {
        STRENGTH,
        DEXTERITY,
        CONSTITUTION,
        WISDOM,
        INTELLIGENCE,
        CHARISMA
    }

    public class StatController : Node
    {
        [Export] private int strength = 10;
        [Export] private int dexterity = 10;
        [Export] private int constitution = 10;

        [Export] private int wisdom = 10;
        [Export] private int intelligence = 10;
        [Export] private int charisma = 10;

        public int GetStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.STRENGTH:
                    return strength;
                case StatType.DEXTERITY:
                    return dexterity;
                case StatType.CONSTITUTION:
                    return constitution;
                case StatType.WISDOM:
                    return wisdom;
                case StatType.INTELLIGENCE:
                    return intelligence;
                case StatType.CHARISMA:
                    return charisma;
            }

            return -1;
        }

        public int GetStatBonus(StatType statType)
        {
            switch (statType)
            {
                case StatType.STRENGTH:
                    return (strength - 10) / 2;
                case StatType.DEXTERITY:
                    return (dexterity - 10) / 2;
                case StatType.CONSTITUTION:
                    return (constitution - 10) / 2;
                case StatType.WISDOM:
                    return (wisdom - 10) / 2;
                case StatType.INTELLIGENCE:
                    return (intelligence - 10) / 2;
                case StatType.CHARISMA:
                    return (charisma - 10) / 2;
            }

            return -1;
        }

        public void SetStat(StatType statType, int value)
        {
            switch (statType)
            {
                case StatType.STRENGTH:
                    strength = value;
                    break;
                case StatType.DEXTERITY:
                    dexterity = value;
                    break;
                case StatType.CONSTITUTION:
                    constitution = value;
                    break;
                case StatType.WISDOM:
                    wisdom = value;
                    break;
                case StatType.INTELLIGENCE:
                    intelligence = value;
                    break;
                case StatType.CHARISMA:
                    charisma = value;
                    break;
            }
        }
    }
}