using System;

namespace TextRoguelike.Model
{
    public class Slime : Enemy
    {
        private int damageReduction = 2;
        
        public Slime(Game gameRef) : base("Слизень", 35, 6, 1, gameRef) { }
        
        public override void TakeDamage(int damage)
        {
            int reducedDamage = Math.Max(0, damage - damageReduction);
            base.TakeDamage(reducedDamage);
            Console.WriteLine($"Слизень уменьшает урон на {damageReduction} единицы!");
        }
    }
}