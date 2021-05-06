using BitRoguelike.Scripts.Items.Equipment;
using Godot;

namespace BitRoguelike.Scripts.Characters.Controllers
{
    public class ArmorController : Node
    {
        [Export] private Armor headArmor = null;
        [Export] private Armor chestArmor = null;
        [Export] private Armor legArmor = null;
        [Export] private Armor handArmor = null;

        public override void _Ready()
        {
            if (GetChildCount() > 0)
            {
                AutoEquip();
            }
        }

        /// <summary>
        /// Parse the child armor and attempt to equip them all. This should only be call on NON-Player characters
        /// </summary>
        private void AutoEquip()
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                Armor a = GetChildOrNull<Armor>(i);
                if (a != null)
                {
                    EquipArmor(new ArmorEquipPayload(a, a.GetArmorSlotType()));
                }
            }
        }

        public void EquipArmor(ArmorEquipPayload payload)
        {
            GD.Print("ArmorController - EquipArmor");
            Armor armor = payload.equipable as Armor;

            switch (armor.GetArmorSlotType())
            {
                case ArmorSlotType.HEAD:
                    if (headArmor != null) UnEquipArmor(payload);
                    headArmor = armor;
                    break;
                case ArmorSlotType.CHEST:
                    if (chestArmor != null) UnEquipArmor(payload);
                    chestArmor = armor;
                    break;
                case ArmorSlotType.LEGS:
                    if (legArmor != null) UnEquipArmor(payload);
                    legArmor = armor;
                    break;
                case ArmorSlotType.HANDS:
                    if (handArmor != null) UnEquipArmor(payload);
                    handArmor = armor;
                    break;
            }
        }

        public void UnEquipArmor(ArmorEquipPayload payload)
        {
            GD.Print("ArmorController - UnEquipArmor");
            switch (payload.targetSlot)
            {
                case ArmorSlotType.HEAD:
                    headArmor = null;
                    break;
                case ArmorSlotType.CHEST:
                    chestArmor = null;
                    break;
                case ArmorSlotType.LEGS:
                    legArmor = null;
                    break;
                case ArmorSlotType.HANDS:
                    handArmor = null;
                    break;
            }
        }

        public int GetArmorDefenseBonus()
        {
            int defenseBonus = 0;

            if (headArmor != null)
                defenseBonus += headArmor.GetDefenseBonus();

            if (chestArmor != null)
                defenseBonus += chestArmor.GetDefenseBonus();

            if (legArmor != null)
                defenseBonus += legArmor.GetDefenseBonus();

            if (handArmor != null)
                defenseBonus += handArmor.GetDefenseBonus();


            return defenseBonus;
        }
    }
}