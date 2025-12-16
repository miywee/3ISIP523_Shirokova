using System;

namespace TextRoguelike.Model
{
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
        
        public virtual void TakeDamage(int damage)
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
}