using System;

namespace TextRoguelike.Model
{
    public class Goblin : Enemy
    {
        private double critChance = 0.2;
        
        public Goblin(Game gameRef) : base("Гоблин", 30, 8, 2, gameRef) { }
        
        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Math.Max(1, Attack - playerDefense);
            
            if (game.random.NextDouble() < critChance)
            {
                Console.WriteLine("Гоблин наносит критический удар!");
                return baseDamage * 2;
            }
            
            return baseDamage;
        }
    }
}