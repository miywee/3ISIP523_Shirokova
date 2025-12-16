namespace TextRoguelike.Model
{
    public class Mage : Enemy
    {
        private double freezeChance = 0.25;
        
        public Mage(Game gameRef) : base("Маг", 20, 12, 1, gameRef) { }
        
        public override bool TrySpecialAbility()
        {
            return game.random.NextDouble() < freezeChance;
        }
    }
}