using System;
using System.Collections.Generic;
using BitRoguelike.Scripts.Items;
using Godot;

namespace BitRoguelike.Scripts.Map.Objects
{
    public enum ChestState
    {
        LOCKED,
        OPENED,
        CLOSED
    }
    
    public class LootChest : Node2D, IInteractable
    {
        public static Action<LootChest> OnChestOpened;
        
        [Export] private bool isLocked = false;
        
        private ChestState currentState = ChestState.CLOSED;

        [Export] private NodePath openedSpritePath = null;
        [Export] private NodePath closedSpritePath = null;
        [Export] private NodePath animationPlayerPath = null;

        private Sprite openedSprite;
        private Sprite closedSprite;
        private AnimationPlayer animationPlayer;

        private List<Item> containedItems = new List<Item>();

        public override void _Ready()
        {
            openedSprite = GetNode<Sprite>(openedSpritePath);
            closedSprite = GetNode<Sprite>(closedSpritePath);
            animationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);

            if (isLocked)
            {
                currentState = ChestState.LOCKED;
            }

            ScanChildren();
        }

        private void ScanChildren()
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i) is Item item)
                {
                    containedItems.Add(item);
                    item.SetEnabled(false);
                }
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

        public List<Item> GetContainedItems() => containedItems;

        public void RemoveItem(Item itemToRemove)
        {
            if (containedItems.Contains(itemToRemove))
            {
                RemoveChild(itemToRemove);
                containedItems.Remove(itemToRemove);
            }
        }
    }
}