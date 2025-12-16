namespace TextRoguelike.Model.Items
{
    public class Armor : Item
    {
        public int Defense { get; set; }
        
        public Armor(string name, int defense, int value) : base(name, value)
        {
            Defense = defense;
        }
        
        public override string GetDescription()
        {
            return $"{Name} (Защита: {Defense}, Ценность: {Value})";
        }
    }
}