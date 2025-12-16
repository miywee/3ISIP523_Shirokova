namespace TextRoguelike.Model
{
    public class Archmage : Mage
    {
        public Archmage(Game gameRef) : base(gameRef)
        {
            Name = "Архимаг C++ (Босс-Маг)";
            MaxHP = (int)(MaxHP * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
        }
        
        public override bool TrySpecialAbility()
        {
            double freezeChance = 0.35;
            return game.random.NextDouble() < freezeChance;
        }
    }
}