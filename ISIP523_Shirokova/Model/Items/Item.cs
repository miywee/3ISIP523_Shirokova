namespace TextRoguelike.Model.Items
{
    public abstract class Item
    {
        public string Name { get; set; }
        public int Value { get; set; }
        
        public Item(string name, int value)
        {
            Name = name;
            Value = value;
        }
        
        public abstract string GetDescription();
    }
}