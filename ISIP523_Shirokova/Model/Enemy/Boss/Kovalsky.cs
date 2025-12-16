namespace TextRoguelike.Model
{
    public class Kovalsky : Skeleton
    {
        public Kovalsky(Game gameRef) : base(gameRef)
        {
            Name = "Ковальский (Босс-Скелет)";
            MaxHP = (int)(MaxHP * 2.5);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }
}