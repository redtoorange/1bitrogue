using BitRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace BitRoguelike.Scripts.UI
{
    public class ResourceBar : Control
    {
        [Export] private bool hideWhenFull = false;
        [Export] private Color baseColor = Colors.Red;
        [Export] private Color gainColor = Colors.Green;
        [Export] private Color loseColor = Colors.Yellow;

        private ResourceController resourceController;

        private TextureProgress baseLayer;
        private TextureProgress gainLayer;
        private TextureProgress loseLayer;

        private Tween tweener = new Tween();

        public override void _Ready()
        {
            baseLayer = GetNode<TextureProgress>("BaseLayer");
            gainLayer = GetNode<TextureProgress>("GainLayer");
            loseLayer = GetNode<TextureProgress>("LoseLayer");

            AddChild(tweener);
            tweener.Connect("tween_completed", this, nameof(OnTweenComplete));

            baseLayer.TintProgress = baseColor;
            loseLayer.TintProgress = loseColor;
            gainLayer.TintProgress = gainColor;
        }

        public void Init(ResourceController resourceController)
        {
            this.resourceController = resourceController;
            this.resourceController.OnResourceChange += HandleValueChange;

            baseLayer.Value = this.resourceController.GetPercentValue() * 100;
            loseLayer.Value = 0;
            gainLayer.Value = 0;

            if (hideWhenFull && baseLayer.Value == 100)
            {
                Hide();
            }
        }

        private void HandleValueChange(ResourceChangeData data)
        {
            float value = (data.newValue / (float) data.maxValue) * 100;
            if (data.changeType == ResourceChangeType.LOSE)
            {
                if (hideWhenFull && value > 0)
                {
                    Show();
                }

                AnimateLoss(value);
            }
            else
            {
                AnimateGain(value);
            }
        }

        private void AnimateLoss(float to)
        {
            loseLayer.Value = baseLayer.Value;
            baseLayer.Value = to;

            tweener.StopAll();
            tweener.InterpolateProperty(loseLayer, "value", loseLayer.Value, baseLayer.Value, 0.25f);
            tweener.Start();
        }

        private void OnTweenComplete(TextureProgress obj, NodePath key)
        {
            loseLayer.Value = 0;
            gainLayer.Value = 0;

            if (hideWhenFull && baseLayer.Value == 100)
            {
                Hide();
            }
        }

        private void AnimateGain(float to)
        {
            gainLayer.Value = to;

            tweener.StopAll();
            tweener.InterpolateProperty(baseLayer, "value", baseLayer.Value, gainLayer.Value, 0.25f);
            tweener.Start();
        }
    }
}