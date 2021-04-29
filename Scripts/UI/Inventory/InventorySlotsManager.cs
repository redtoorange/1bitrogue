using GameboyRoguelike.Scripts.UI.Inventory.Slots;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class InventorySlotsManager : PanelContainer
    {
        private BackPackSlot[,] backPackSlots;

        public override void _Ready()
        {
            backPackSlots = new BackPackSlot[4, 4];

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    BackPackSlot slot = GetNode<BackPackSlot>($"Container/Rows/{row}/{col}");
                    slot.Init(row, col);
                    backPackSlots[row, col] = slot;
                }
            }
        }
        
        public void Init()
        {
            
        }
    }
}