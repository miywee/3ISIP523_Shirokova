using System;

namespace TextRoguelike.Model
{
    public class VVG : Goblin
    {
        public VVG(Game gameRef) : base(gameRef)
        {
            Name = "ВВГ (Босс-Гоблин)";
            MaxHP = (int)(MaxHP * 2.0);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }
        
        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Math.Max(1, Attack - playerDefense);
            double critChance = 0.3;
            
            if (game.random.NextDouble() < critChance)
            {
                Console.WriteLine("ВВГ наносит критический удар!");
                return baseDamage * 2;
            }
            
            return baseDamage;
        }
    }
}