using GameboyRoguelike.Scripts.Map.Objects;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class ChestUI : Node
    {
        [Export] private NodePath windowDialogPath;
        private WindowDialog windowDialog;
        
        private BedroomChest currentChest;

        
        public override void _Ready()
        {
            windowDialog = GetNode<WindowDialog>(windowDialogPath);

            windowDialog.Connect("popup_hide", this, nameof(OnWindowClosed));
        }

        public override void _EnterTree()
        {
            BedroomChest.OnChestOpened += HandleOnChestOpened;
        }

        public override void _ExitTree()
        {
            BedroomChest.OnChestOpened -= HandleOnChestOpened;
        }

        private void HandleOnChestOpened(BedroomChest chest)
        {
            currentChest = chest;
            windowDialog.PopupCentered();
        }

        private void OnWindowClosed()
        {
            GD.Print("Closing");
            currentChest.Close();
        }
    }
}