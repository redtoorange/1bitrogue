using GameboyRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace GameboyRoguelike.Scripts.UI
{
    public class ResourceBar : Control
    {
        [Export] private Color baseColor = Colors.Red;
        [Export] private Color gainColor = Colors.Green;
        [Export] private Color loseColor = Colors.Yellow;

        [Export] private float tweenSpeed = 0.25f;
        
        private ResourceController resourceController;

        private TextureProgress baseLayer;
        private TextureProgress gainLayer;
        private TextureProgress loseLayer;

        private Tween tweener = new Tween();

        private float baseValue = 100;
        private float gainValue = 0;
        private float loseValue = 0;

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

            baseValue = this.resourceController.GetPercentValue() * 100;
            baseLayer.Value = baseValue;
        }

        private void HandleValueChange(ResourceChangeData data)
        {
            // SetValue((data.newValue / (float) data.maxValue) * 100);
            
            AnimateLoss((data.newValue / (float) data.maxValue) * 100);
        }

        private void AnimateLoss(float to)
        {
            loseValue = baseValue;
            loseLayer.Value = loseValue;

            baseValue = to;
            baseLayer.Value = baseValue;

            tweener.StopAll();
            tweener.InterpolateProperty(loseLayer, "value", loseValue, baseValue, 0.25f);
            tweener.Start();
        }

        private void OnTweenComplete(TextureProgress obj, NodePath key)
        {
            GD.Print("Tween Completed: <" + obj.Name + ">, <" + key + ">.");
        }

        private void AnimateGain(float to)
        {
        }

        private void SetValue(float amount)
        {
            baseLayer.Value = amount;
        }
    }
}