using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public class CharacterBase
    {
        public int Health 
        {
            get
            {
                return health;
            } 
            set 
            { 
                health = value;
                OnHealthChanged?.Invoke(this);
            } 
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
                OnMaxHealthChanged?.Invoke(this);
            }
        }

        public int Armor
        {
            get
            {
                return armor;
            }
            set
            {
                armor = value;
                OnArmorChanged?.Invoke(this);
            }
        }



        public System.Action<CharacterBase> OnHealthChanged;
        public System.Action<CharacterBase> OnMaxHealthChanged;
        public System.Action<CharacterBase> OnArmorChanged;
        public List<Skill> Skills;
        public string Name;

        private int health;
        private int maxHealth;
        private int armor;

    }
}