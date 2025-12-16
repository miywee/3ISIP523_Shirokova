using System;
using System.Collections.Generic;

namespace TextRoguelike.Model.Items
{
    public static class ItemGenerator
    {
        public static Item GenerateRandomItem(Game game)
        {
            int choice = game.random.Next(3);
            
            switch (choice)
            {
                case 0:
                    return GenerateRandomWeapon(game);
                case 1:
                    return GenerateRandomArmor(game);
                case 2:
                    return new Potion();
                default:
                    return new Potion();
            }
        }
        
        public static Weapon GenerateRandomWeapon(Game game)
        {
            List<Weapon> weapons = new List<Weapon>
            {
                new Weapon("Кинжал", 3, 2),
                new Weapon("Длинный меч", 10, 6),
                new Weapon("Волшебный посох", 15, 12)
            };
            
            return weapons[game.random.Next(weapons.Count)];
        }
        
        public static Armor GenerateRandomArmor(Game game)
        {
            List<Armor> armors = new List<Armor>
            {
                new Armor("Кожаный доспех", 3, 2),
                new Armor("Кольчуга", 6, 4),
                new Armor("Доспехи", 15, 12)
            };
            
            return armors[game.random.Next(armors.Count)];
        }
    }
}