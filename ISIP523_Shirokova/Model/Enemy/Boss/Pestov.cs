namespace TextRoguelike.Model
{
    public class Pestov : Enemy
    {
        private double freezeChance = 0.4;
        
        public Pestov(Game gameRef) : base("Пестов C-- (Босс-Скелет)", 33, 18, 2, gameRef) 
        {
        }
        
        public override bool TrySpecialAbility()
        {
            return game.random.NextDouble() < freezeChance;
        }
        
        public override int CalculateDamage(int playerDefense)
        {
            return Math.Max(1, Attack);
        }
    }
}