using System;

namespace TextRoguelike.Model
{
    public static class EnemyFactory
    {
        public static Enemy CreateEnemy(string enemyType, Game game)
        {
            switch (enemyType.ToLower())
            {
                case "goblin":
                    return new Goblin(game);
                case "skeleton":
                    return new Skeleton(game);
                case "mage":
                    return new Mage(game);
                case "slime":
                    return new Slime(game);
                default:
                    throw new ArgumentException($"Неизвестный тип врага: {enemyType}");
            }
        }
        
        public static Enemy CreateRandomEnemy(Game game)
        {
            int choice = game.random.Next(4);
            
            switch (choice)
            {
                case 0: 
                    return new Goblin(game);
                case 1: 
                    return new Skeleton(game);
                case 2: 
                    return new Mage(game);
                case 3: 
                    return new Slime(game);
                default: 
                    return new Goblin(game);
            }
        }
        
        public static Enemy CreateRandomBoss(Game game)
        {
            int choice = game.random.Next(4);
            
            switch (choice)
            {
                case 0: return new VVG(game);
                case 1: return new Kovalsky(game);
                case 2: return new Archmage(game);
                case 3: return new Pestov(game);
                default: return new VVG(game);
            }
        }
    }
}