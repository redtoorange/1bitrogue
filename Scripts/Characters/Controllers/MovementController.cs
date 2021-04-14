using Godot;

namespace GameboyRoguelike.Scripts.Characters.Controllers
{
    public class MovementController : Node
    {
        protected AnimationPlayer animationPlayer = null;
        protected Tween tween = null;
        protected RayCast2D rayCast2D = null;
        protected bool isTweening = false;

        public void Init(AnimationPlayer animationPlayer, RayCast2D rayCast2D, Tween tween)
        {
            this.animationPlayer = animationPlayer;
            this.rayCast2D = rayCast2D;
            this.tween = tween;

            tween.Connect("tween_completed", this, nameof(TweenCompleted));
        }
    
        private void TweenCompleted(Object obj, NodePath key)
        {
            isTweening = false;
        }
    }
}