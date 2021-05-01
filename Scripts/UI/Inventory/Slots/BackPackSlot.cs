using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.Slots
{
    public class BackPackSlot : ItemSlot
    {
        private int row = -1;
        private int col = -1;

        private ItemInventoryTile currentTile = null;
        
        public void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        
        protected override void HoverStarted(){
            GD.Print($"HoverStarted ({row}, {col})");
        }

        protected override void HoverEnded()
        {
            GD.Print($"HoverEnded ({row}, {col})");
        }

        public override void AddItemTile(ItemInventoryTile tile)
        {
            tile.SetSlot(this);
            currentTile = tile;

            AddChild(tile);
        }

        public override void RemoveItemTile(ItemInventoryTile tile)
        {
            throw new System.NotImplementedException();
        }

        public bool IsOccupied() => currentTile != null;
    }
}