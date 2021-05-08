using System;
using Godot;

namespace BitRoguelike.Scripts.UI
{
    public class PauseMenuController : Control
    {
        public static Action OnQuitToMainMenu;
        public Action OnMenuClosed;
        
        private void OnContinueClicked()
        {
            OnMenuClosed?.Invoke();
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
            if (OnQuitToMainMenu == null)
            {
                GetTree().Quit();
            }
            else
            {
                OnQuitToMainMenu.Invoke();
            }
        }
    }
}