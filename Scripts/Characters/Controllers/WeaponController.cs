using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public class WeaponController : Node
    {
        [Export] private int baseUnarmedDamage = 5;

        [Export] private Weapon mainHand = null;
        [Export] private Weapon offHand = null;

        public override void _Ready()
        {
            if (GetChildCount() > 0)
            {
                AutoEquip();
            }
        }

        /// <summary>
        /// Parse the child weapons and attempt to equip them all. This should only be call on NON-Player characters
        /// </summary>
        private void AutoEquip()
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                Weapon w = GetChildOrNull<Weapon>(i);
                if (w != null)
                {
                    if (w.GetEquipType() == WeaponEquipType.OFF_HAND)
                    {
                        EquipWeapon(new WeaponEquipPayload(w, WeaponSlotType.OFF_HAND));
                    }
                    else
                    {
                        EquipWeapon(new WeaponEquipPayload(w, WeaponSlotType.MAIN_HAND));
                    }
                }
            }
        }

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

        public void EquipWeapon(WeaponEquipPayload payload)
        {
            GD.Print("WeaponController - EquipWeapon");
            Weapon weapon = payload.equipable as Weapon;
            WeaponStats weaponData = weapon.GetStats();
            WeaponEquipType equipType = weaponData.weaponEquipType;

            if (equipType == WeaponEquipType.MAIN_HAND && payload.targetSlotType == WeaponSlotType.MAIN_HAND)
            {
                UnEquipWeapon(payload);
                mainHand = weapon;
            }
            else if (equipType == WeaponEquipType.OFF_HAND && payload.targetSlotType == WeaponSlotType.OFF_HAND)
            {
                UnEquipWeapon(payload);
                offHand = weapon;
            }
            else if (equipType == WeaponEquipType.TWO_HANDS)
            {
                UnEquipWeapon(payload);
                mainHand = weapon;
                offHand = weapon;
            }
            else if (equipType == WeaponEquipType.EITHER_HAND)
            {
                UnEquipWeapon(payload);
                if (payload.targetSlotType == WeaponSlotType.MAIN_HAND)
                {
                    mainHand = weapon;
                }
                else if (payload.targetSlotType == WeaponSlotType.OFF_HAND)
                {
                    offHand = weapon;
                }
            }
        }

        public void UnEquipWeapon(WeaponEquipPayload payload)
        {
            GD.Print("WeaponController - UnEquipWeapon");
            if (mainHand != null && mainHand.GetStats().weaponEquipType == WeaponEquipType.TWO_HANDS)
            {
                mainHand = null;
                offHand = null;
            }
            else
            {
                if (payload.targetSlotType == WeaponSlotType.MAIN_HAND)
                {
                    mainHand = null;
                }
                else if (payload.targetSlotType == WeaponSlotType.OFF_HAND)
                {
                    offHand = null;
                }
            }
        }
    }
}