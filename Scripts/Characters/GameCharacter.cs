using System;
using GameboyRoguelike.Scripts.Characters.Controllers;
using Godot;

namespace GameboyRoguelike.Scripts.Characters
{
    public enum CharacterType
    {
        PLAYER,
        ENEMY,
        NPC
    }

    public class GameCharacter : KinematicBody2D
    {
        [Export] private CharacterType characterType = CharacterType.NPC;
        
        private AnimationPlayer animationPlayer;
        private RayCast2D rayCast2D;
        private Tween tween;
        private StatController statController;
        private ArmorController armorController;
        private WeaponController weaponController;

        public CharacterType GetCharacterType() => characterType;
        public AnimationPlayer GetAnimationPlayer() => animationPlayer;
        public RayCast2D GetRayCast2D() => rayCast2D;
        public Tween GetTween() => tween;
        public WeaponController GetWeaponController() => weaponController;
        public ArmorController GetArmorController() => armorController;
        public StatController GetStatController() => statController;

        public override void _Ready()
        {
            base._Ready();

            animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            rayCast2D = GetNode<RayCast2D>("RayCaster");
            tween = GetNode<Tween>("Tween");
            statController = GetNode<StatController>("StatController");
            weaponController = GetNode<WeaponController>("WeaponController");
            armorController = GetNode<ArmorController>("ArmorController");
        }
    }
}