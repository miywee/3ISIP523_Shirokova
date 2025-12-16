using System;
using TextRoguelike.Model.Items;

namespace TextRoguelike.Model
{
    public class Game
    {
        public Player player;
        public Random random;
        public int turnCount;
        
        public Game()
        {
            random = new Random();
            player = new Player(this);
            turnCount = 0;
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в мини-игру рогалик!");
            Console.WriteLine("Каждый ход вы будете встречать либо сундук, либо врага. А каждые 10 шагов вас ждет встреча с боссом\n");
            
            while (player.IsAlive)
            {
                turnCount++;
                Console.WriteLine($"\nХод {turnCount}");
                Console.WriteLine(player.GetStatus());
                
                if (turnCount % 10 == 0)
                {
                    Console.WriteLine("Появляется босс");
                    FightBoss();
                }
                else
                {
                    if (random.Next(2) == 0)
                    {
                        FightEnemy();
                    }
                    else
                    {
                        OpenChest();
                    }
                }
                
                if (player.IsAlive)
                {
                    Console.WriteLine("\nНажмите любую клавишу");
                    Console.ReadKey();
                }
            }
            
            Console.WriteLine("\nИГРА ОКОНЧЕНА");
            Console.WriteLine($"Вы продержались {turnCount} ходов!");
        }
        
        public void FightEnemy()
        {
            Enemy enemy = EnemyFactory.CreateRandomEnemy(this);
            Console.WriteLine($"Вы встретили: {enemy.GetInfo()}");
            
            bool playerTurn = true;
            bool playerFrozen = false;
            
            while (enemy.IsAlive && player.IsAlive)
            {
                if (playerTurn && !playerFrozen)
                {
                    PlayerTurn(enemy);
                }
                else if (playerTurn && playerFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    playerFrozen = false;
                }
                
                if (enemy.IsAlive && player.IsAlive)
                {
                    EnemyTurn(enemy, ref playerFrozen);
                }
                
                playerTurn = !playerTurn;
            }
            
            if (!enemy.IsAlive)
            {
                Console.WriteLine($"Вы победили {enemy.Name}!");
            }
        }
        
        public void FightBoss()
        {
            Enemy boss = EnemyFactory.CreateRandomBoss(this);
            Console.WriteLine($"Появляется босс: {boss.GetInfo()}");
            
            bool playerTurn = true;
            bool playerFrozen = false;
            
            while (boss.IsAlive && player.IsAlive)
            {
                if (playerTurn && !playerFrozen)
                {
                    PlayerTurn(boss);
                }
                else if (playerTurn && playerFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    playerFrozen = false;
                }
                
                if (boss.IsAlive && player.IsAlive)
                {
                    EnemyTurn(boss, ref playerFrozen);
                }
                
                playerTurn = !playerTurn;
            }
            
            if (!boss.IsAlive)
            {
                Console.WriteLine($"Вы победили босса {boss.Name}! Поздравляем!");
            }
        }
        
        public void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\nВаш ход:");
            Console.WriteLine("1 - Атаковать");
            Console.WriteLine("2 - Защищаться");
            Console.Write("Выберите действие: ");
            
            string input = Console.ReadLine();
            
            switch (input)
            {
                case "1":
                    int damage = player.CalculateDamage();
                    enemy.TakeDamage(damage);
                    Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
                    break;
                    
                case "2":
                    Console.WriteLine("Вы готовитесь к защите...");
                    break;
                    
                default:
                    Console.WriteLine("Неверный ввод, вы пропускаете ход!");
                    break;
            }
        }
        
        public void EnemyTurn(Enemy enemy, ref bool playerFrozen)
        {
            Console.WriteLine($"\nХод {enemy.Name}:");
            
            if (enemy.TrySpecialAbility())
            {
                Console.WriteLine($"{enemy.Name} накладывает заморозку!");
                playerFrozen = true;
            }
            
            int enemyDamage = enemy.CalculateDamage(player.TotalDefense);
            
            if (player.TryDodge())
            {
                Console.WriteLine("Вы уверенно уклоняетесь от атаки!");
            }
            else
            {
                int finalDamage = player.CalculateBlock(enemyDamage);
                player.TakeDamage(finalDamage);
                Console.WriteLine($"{enemy.Name} наносит вам {finalDamage} урона!");
            }
            
            Console.WriteLine($"Ваше здоровье: {player.CurrentHP}/{player.MaxHP}");
        }
        
        public void OpenChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            Item item = ItemGenerator.GenerateRandomItem(this);
            Console.WriteLine($"Внутри: {item.GetDescription()}");
            
            if (item is Potion potion)
            {
                player.Heal();
                Console.WriteLine("Вы выпили зелье и полностью восстановили здоровье!");
            }
            else if (item is Weapon weapon)
            {
                Console.WriteLine("\nВаше текущее оружие:");
                Console.WriteLine(player.CurrentWeapon.GetDescription());
                Console.WriteLine("\nНайденное оружие:");
                Console.WriteLine(weapon.GetDescription());
                
                Console.Write("Взять новое оружие? (y/n): ");
                string input = Console.ReadLine();
                
                if (input.ToLower() == "y")
                {
                    player.EquipWeapon(weapon);
                    Console.WriteLine($"Вы экипировали {weapon.Name}!");
                }
            }
            else if (item is Armor armor)
            {
                Console.WriteLine("\nВаши текущие доспехи:");
                Console.WriteLine(player.CurrentArmor.GetDescription());
                Console.WriteLine("\nНайденные доспехи:");
                Console.WriteLine(armor.GetDescription());
                
                Console.Write("Взять новые доспехи? (y/n): ");
                string input = Console.ReadLine();
                
                if (input.ToLower() == "y")
                {
                    player.EquipArmor(armor);
                    Console.WriteLine($"Вы экипировали {armor.Name}!");
                }
            }
        }
    }
}