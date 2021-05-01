using Godot;

namespace BitRoguelike.Scripts.UI.HotKey
{
    [Tool]
    public class HotKeySlot : Control
    {
        [Export] private string labelText = "1";
        [Export] private bool enabled = true;
        
        private NinePatchRect border;
        private NinePatchRect filling;
        private Label label;

        public override void _Ready()
        {
            border = GetNode<NinePatchRect>("Border");
            filling = GetNode<NinePatchRect>("Filling");
            label = GetNode<Label>("Label");

            label.Text = labelText;
            
            SetEnabled(!enabled);
        }

        public void SetEnabled(bool isEnabled)
        {
            if (isEnabled != enabled)
            {
                enabled = isEnabled;
                Color borderColor = border.Modulate;
                Color fillingColor = filling.Modulate;
                if (enabled)
                {
                    borderColor.a = 0.5f;
                    fillingColor.a = 0.5f;
                }
                else
                {
                    borderColor.a = 1;
                    fillingColor.a = 1;
                }

                border.Modulate = borderColor;
                filling.Modulate = fillingColor;
            }
        }
    }
}