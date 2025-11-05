using System;
using System.Collections.Generic;

namespace TextRoguelike
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
    
    public abstract class Enemy
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        
        public Game game;
        
        public Enemy(string name, int hp, int attack, int defense, Game gameRef)
        {
            Name = name;
            MaxHP = hp;
            CurrentHP = hp;
            Attack = attack;
            Defense = defense;
            game = gameRef;
        }
        
        public virtual int CalculateDamage(int playerDefense)
        {
            return Math.Max(1, Attack - playerDefense);
        }
        
        public virtual bool TrySpecialAbility()
        {
            return false;
        }
        
        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }
        
        public bool IsAlive => CurrentHP > 0;
        
        public virtual string GetInfo()
        {
            return $"{Name} (HP: {CurrentHP}/{MaxHP}, Атака: {Attack}, Защита: {Defense})";
        }
    }
    
    public class Goblin : Enemy
    {
        private double critChance = 0.2;
        
        public Goblin(Game gameRef) : base("Гоблин", 30, 8, 2, gameRef) { }
        
        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Math.Max(1, Attack - playerDefense);
            
            if (game.random.NextDouble() < critChance)
            {
                Console.WriteLine("Гоблин наносит критический удар!");
                return baseDamage * 2;
            }
            
            return baseDamage;
        }
    }
    
    public class Skeleton : Enemy
    {
        public Skeleton(Game gameRef) : base("Скелет", 25, 10, 3, gameRef) { }
        
        public override int CalculateDamage(int playerDefense)
        {
            return Math.Max(1, Attack);
        }
    }
    
    public class Mage : Enemy
    {
        private double freezeChance = 0.25;
        
        public Mage(Game gameRef) : base("Маг", 20, 12, 1, gameRef) { }
        
        public override bool TrySpecialAbility()
        {
            return game.random.NextDouble() < freezeChance;
        }
    }
    
    public class VVG : Goblin
    {
        public VVG(Game gameRef) : base(gameRef)
        {
            Name = "ВВГ (Босс-Гоблин)";
            MaxHP = (int)(MaxHP * 2.0);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }
        
        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Math.Max(1, Attack - playerDefense);
            double critChance = 0.3;
            
            if (game.random.NextDouble() < critChance)
            {
                Console.WriteLine("ВВГ наносит критический удар!");
                return baseDamage * 2;
            }
            
            return baseDamage;
        }
    }

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

    public class Pestov : Skeleton
    {
        private double freezeChance = 0.4;
        
        public Pestov(Game gameRef) : base(gameRef)
        {
            Name = "Пестов C-- (Босс-Скелет)";
            MaxHP = (int)(MaxHP * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }
        
        public override bool TrySpecialAbility()
        {
            return game.random.NextDouble() < freezeChance;
        }
    }
    
    public class Player
    {
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public Armor CurrentArmor { get; set; }
        public int TotalAttack => CurrentWeapon?.Attack ?? 5;
        public int TotalDefense => CurrentArmor?.Defense ?? 2;
        public bool IsFrozen { get; set; }
        
        public Game game;
        
        public Player(Game gameRef)
        {
            MaxHP = 100;
            CurrentHP = MaxHP;
            game = gameRef;
            
            CurrentWeapon = new Weapon("Ржавый меч", 5, 1);
            CurrentArmor = new Armor("Потрёпанная кожаная броня", 2, 1);
        }
        
        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }
        
        public void Heal()
        {
            CurrentHP = MaxHP;
        }
        
        public bool IsAlive => CurrentHP > 0;
        
        public int CalculateDamage()
        {
            return TotalAttack;
        }
        
        public bool TryDodge()
        {
            return game.random.NextDouble() < 0.4;
        }
        
        public int CalculateBlock(int incomingDamage)
        {
            double blockPercentage = game.random.Next(70, 101) / 100.0;
            int blockedDamage = (int)(TotalDefense * blockPercentage);
            return Math.Max(0, incomingDamage - blockedDamage);
        }
        
        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
        }
        
        public void EquipArmor(Armor armor)
        {
            CurrentArmor = armor;
        }
        
        public string GetStatus()
        {
            return $"Игрок: HP {CurrentHP}/{MaxHP}, Атака: {TotalAttack}, Защита: {TotalDefense}";
        }
        
        public string GetEquipment()
        {
            return $"Оружие: {CurrentWeapon?.GetDescription() ?? "Нет"}\nДоспехи: {CurrentArmor?.GetDescription() ?? "Нет"}";
        }
    }
    
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
    
    public class Potion : Item
    {
        public Potion() : base("Лечебное зелье", 3) { }
        
        public override string GetDescription()
        {
            return $"{Name} (Восстанавливает всё здоровье, Ценность: {Value})";
        }
    }
    
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
            Enemy enemy = GenerateRandomEnemy();
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
            Enemy boss = GenerateRandomBoss();
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
            
            if (enemy is Mage mage && mage.TrySpecialAbility())
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
        
        public Enemy GenerateRandomEnemy()
        {
            int choice = random.Next(3);
            
            switch (choice)
            {
                case 0: return new Goblin(this);
                case 1: return new Skeleton(this);
                case 2: return new Mage(this);
                default: return new Goblin(this);
            }
        }
        
        public Enemy GenerateRandomBoss()
        {
            int choice = random.Next(4);
            
            switch (choice)
            {
                case 0: return new VVG(this);
                case 1: return new Kovalsky(this);
                case 2: return new Archmage(this);
                case 3: return new Pestov(this);
                default: return new VVG(this);
            }
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
    
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}