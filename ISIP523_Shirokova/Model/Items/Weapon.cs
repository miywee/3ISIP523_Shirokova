namespace TextRoguelike.Model.Items
{
    public class Weapon : Item
    {
        public int Attack { get; set; }
        
        public Weapon(string name, int attack, int value) : base(name, value)
        {
            Attack = attack;
        }
        
        public override string GetDescription()
        {
            return $"{Name} (Атака: {Attack}, Ценность: {Value})";
        }
    }
}