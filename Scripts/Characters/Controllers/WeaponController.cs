using GameboyRoguelike.Scripts.Items;
using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class WeaponController : Node
    {
        [Export] private int baseUnarmedDamage = 5;

        [Export] private Weapon mainHand = null;
        [Export] private Weapon offHand = null;

        public int GetTotalDamage()
        {
            if (mainHand != null || offHand != null)
            {
                int damage = 0;

                if (mainHand != null)
                {
                    damage += mainHand.GetDamage();
                }

                if (offHand != null)
                {
                    damage += offHand.GetDamage();
                }

                return damage;
            }

            return baseUnarmedDamage;
        }

        public void EquipWeapon(Weapon weapon, WeaponSlot targetSlot)
        {
            WeaponStats weaponData = weapon.GetStats();
            WeaponEquipType equipType = weaponData.weaponEquipType;

            if (equipType == WeaponEquipType.MAIN_HAND && targetSlot == WeaponSlot.MAIN_HAND)
            {
                UnEquipWeapon(WeaponSlot.MAIN_HAND);
                mainHand = weapon;
            }
            else if (equipType == WeaponEquipType.OFF_HAND && targetSlot == WeaponSlot.OFF_HAND)
            {
                UnEquipWeapon(WeaponSlot.OFF_HAND);
                offHand = weapon;
            }
            else if (equipType == WeaponEquipType.TWO_HANDS)
            {
                UnEquipWeapon(WeaponSlot.MAIN_HAND);
                mainHand = weapon;
                offHand = weapon;
            }
            else if (equipType == WeaponEquipType.EITHER_HAND)
            {
                UnEquipWeapon(targetSlot);
                if (targetSlot == WeaponSlot.MAIN_HAND)
                {
                    mainHand = weapon;
                }
                else if (targetSlot == WeaponSlot.OFF_HAND)
                {
                    offHand = weapon;
                }
            }
        }

        public void UnEquipWeapon(WeaponSlot slot)
        {
            if (mainHand.GetStats().weaponEquipType == WeaponEquipType.TWO_HANDS)
            {
                mainHand = null;
                offHand = null;
            }
            else
            {
                if (slot == WeaponSlot.MAIN_HAND)
                {
                    mainHand = null;
                }
                else if (slot == WeaponSlot.OFF_HAND)
                {
                    offHand = null;
                }
            }
        }
    }
}