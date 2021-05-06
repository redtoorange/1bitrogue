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
        
        public void EquipArmor(Armor armor)
        {
            
        }

        public void UnEquipArmor(ArmorSlotType slotType)
        {
            
        }
    }
}