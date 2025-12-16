using System;
using TextRoguelike.Model.Items;

namespace TextRoguelike.Model
{
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
}