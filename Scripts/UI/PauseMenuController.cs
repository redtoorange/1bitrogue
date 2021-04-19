using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PauseMenuController : Control
    {
        private void OnContinueClicked()
        {
            GD.Print("Continue Clicked");
        }

        private void OnSettingsClicked()
        {
            GD.Print("Settings Clicked");
        }

        private void OnLoadClicked()
        {
            GD.Print("Load Clicked");
        }

        private void OnSaveClicked()
        {
            GD.Print("Save Clicked");
        }

        private void OnMainMenuClicked()
        {
            GD.Print("Main Menu Clicked");
        }
    }
}