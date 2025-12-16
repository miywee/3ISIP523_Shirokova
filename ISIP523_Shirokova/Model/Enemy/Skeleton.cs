namespace TextRoguelike.Model
{
    public class Skeleton : Enemy
    {
        public Skeleton(Game gameRef) : base("Скелет", 25, 10, 3, gameRef) { }
        
        public override int CalculateDamage(int playerDefense)
        {
            return Math.Max(1, Attack);
        }
    }
}