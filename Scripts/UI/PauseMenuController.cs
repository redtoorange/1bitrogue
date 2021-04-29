﻿using System;
using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory
{
    public class PauseMenuController : Control
    {
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
            GetTree().Quit();
        }
    }
}