using BitRoguelike.Scripts.Characters;
using Godot;

namespace BitRoguelike.Scripts.Systems
{
    public class AttackSystem : Node
    {
        public static AttackSystem S = null;

        private RandomNumberGenerator rng;

        public override void _EnterTree()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                GD.PrintErr("Multiple AttackSystem present");
                QueueFree();
            }

            rng = new RandomNumberGenerator();
            rng.Randomize();
        }

        public void ResolveAttack(IAttacker attacker, IDefender target)
        {
            bool hit = CalculateHit(attacker, target);
            if (hit)
            {
                int damage = CalculateDamage(attacker);
                target.TakeDamage(damage);
            }
        }

        private int CalculateDamage(IAttacker attacker)
        {
            return attacker.GetWeapon() + attacker.GetDamageBonus();
        }

        private bool CalculateHit(IAttacker attacker, IDefender target)
        {
            int armorClass = target.GetArmorClass();
            int hitBonus = attacker.GetHitBonus();
            int roll = rng.RandiRange(1, 20);

            return (roll + hitBonus) >= armorClass;
        }
    }
}