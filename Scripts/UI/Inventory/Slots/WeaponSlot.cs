using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class WeaponSlot : ItemSlot
    {
        [Export] private Items.WeaponSlot weaponSlotType;
        
        protected override void HoverStarted(){
            GD.Print($"HoverStarted {weaponSlotType}");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded {weaponSlotType}");
        }
    }
}