﻿namespace BitRoguelike.Scripts.UI.Inventory.Slots
{
    public class BackPackSlot : ItemSlot
    {
        private int row = -1;
        private int col = -1;
        
        public void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}