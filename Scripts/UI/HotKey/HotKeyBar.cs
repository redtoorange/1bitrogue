using Godot;

namespace BitRoguelike.Scripts.UI.HotKey
{
    public class HotKeyBar : Control
    {
        private HotKeySlot slot1;
        private HotKeySlot slot2;
        private HotKeySlot slot3;
        private HotKeySlot slot4;
        private HotKeySlot slot5;
        private HotKeySlot slot6;
        private HotKeySlot slot7;
        private HotKeySlot slot8;

        public override void _Ready()
        {
            slot1 = GetNode<HotKeySlot>("Slots/HotKeyBox1");
            slot2 = GetNode<HotKeySlot>("Slots/HotKeyBox2");
            slot3 = GetNode<HotKeySlot>("Slots/HotKeyBox3");
            slot4 = GetNode<HotKeySlot>("Slots/HotKeyBox4");
            slot5 = GetNode<HotKeySlot>("Slots/HotKeyBox5");
            slot6 = GetNode<HotKeySlot>("Slots/HotKeyBox6");
            slot7 = GetNode<HotKeySlot>("Slots/HotKeyBox7");
            slot8 = GetNode<HotKeySlot>("Slots/HotKeyBox8");
        }
    }
}