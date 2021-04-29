﻿using GameboyRoguelike.Scripts.Items.Stats;
using Godot;

namespace GameboyRoguelike.Scripts.Items
{
    public enum ArmorSlotType
    {
        HEAD,
        CHEST,
        HANDS,
        LEGS
    }
    
    public class Armor : Item, Equipable
    {
        private ArmorStats armorStats;

        public override void _Ready()
        {
            base._Ready();

            armorStats = stats as ArmorStats;
            if (armorStats == null)
            {
                GD.PrintErr("Failed to convert ItemStat to ArmorStats.");
            }
        }

        public ArmorStats GetStats() => armorStats;
    }
}