namespace TextRoguelike.Model.Items
{
    public class Potion : Item
    {
        public Potion() : base("Лечебное зелье", 3) { }
        
        public override string GetDescription()
        {
            return $"{Name} (Восстанавливает всё здоровье, Ценность: {Value})";
        }
    }
}