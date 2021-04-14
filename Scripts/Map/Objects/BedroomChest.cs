using System;
using Godot;

namespace GameboyRoguelike.Scripts.Map.Objects
{
    public enum ChestState
    {
        LOCKED,
        OPENED,
        CLOSED
    }
    
    public class BedroomChest : Node2D, IInteractable
    {
        [Signal] public delegate void OnChestOpenedSignal();
        public static Action<BedroomChest> OnChestOpened;
        
        [Export] private bool isLocked = false;
        
        private ChestState currentState = ChestState.CLOSED;

        [Export] private NodePath openedSpritePath = null;
        [Export] private NodePath closedSpritePath = null;
        [Export] private NodePath animationPlayerPath = null;

        private Sprite openedSprite;
        private Sprite closedSprite;
        private AnimationPlayer animationPlayer;

        public override void _Ready()
        {
            openedSprite = GetNode<Sprite>(openedSpritePath);
            closedSprite = GetNode<Sprite>(closedSpritePath);
            animationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);

            if (isLocked)
            {
                currentState = ChestState.LOCKED;
            }
        }

        public void Unlock()
        {
            animationPlayer.Play("UnlockedText");
            isLocked = false;
            currentState = ChestState.CLOSED;
        }

        public void Open()
        {
            openedSprite.Visible = true;
            closedSprite.Visible = false;
            
            OnChestOpened?.Invoke(this);
            EmitSignal(nameof(OnChestOpenedSignal));
        }

        public void Close()
        {
            closedSprite.Visible = true;
            openedSprite.Visible = false;
        }
        
        public void Interact()
        {
            if (currentState == ChestState.LOCKED)
            {
                if (HasKey())
                {
                    Unlock();
                }
                else
                {
                    animationPlayer.Play("LockedText");
                }
            }
            else if (currentState == ChestState.CLOSED)
            {
                Open();
            }
        }

        public bool HasKey()
        {
            return false;
        }
    }
}