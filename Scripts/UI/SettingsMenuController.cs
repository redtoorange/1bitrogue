using System;
using Godot;

namespace BitRoguelike.Scripts.UI
{
    [Serializable]
    public class SettingsState
    {
        public bool fullscreen = false;

        public static SettingsState Clone(SettingsState other)
        {
            SettingsState newState = new SettingsState();

            newState.fullscreen = other.fullscreen;

            return newState;
        }
    }

    public class SettingsMenuController : Control
    {
        public Action OnSettingsMenuBackPressed;
        
        [Export] private NodePath fullScreenBoxPath = null;
        [Export] private NodePath applyButtonPath = null;
        [Export] private NodePath backButtonPath = null;

        private CheckBox fullscreenBox;
        private Button applyButton;
        private Button backButton;

        private SettingsState savedState;
        private SettingsState tempState;

        public override void _Ready()
        {
            LoadSettings();
            tempState = new SettingsState();

            fullscreenBox = GetNode<CheckBox>(fullScreenBoxPath);
            applyButton = GetNode<Button>(applyButtonPath);
            backButton = GetNode<Button>(backButtonPath);

            fullscreenBox.Connect("toggled", this, nameof(HandleFullScreenToggled));
            applyButton.Connect("pressed", this, nameof(HandleApplyPressed));
            backButton.Connect("pressed", this, nameof(HandleBackPressed));
        }

        public override void _Input(InputEvent @event)
        {
            if (IsVisibleInTree())
            {
                if (@event is InputEventKey iek && iek.IsActionPressed("Back"))
                {
                    HandleBackPressed();
                }
            }
        }

        private void LoadSettings()
        {
            savedState = new SettingsState();
        }

        private void HandleFullScreenToggled(bool pressed)
        {
            tempState.fullscreen = pressed;
        }

        private void HandleBackPressed()
        {
            tempState = SettingsState.Clone(savedState);

            fullscreenBox.Pressed = tempState.fullscreen;
            
            OnSettingsMenuBackPressed?.Invoke();
        }

        private void HandleApplyPressed()
        {
            savedState = SettingsState.Clone(tempState);
            
            OS.WindowFullscreen = savedState.fullscreen;
        }
    }
}