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

                if (IsDead)
                {
                    OnDied?.Invoke(this);
                }
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

        public bool IsDead
        {
            get
            {
                return health <= 0;
            }
        }

        public System.Action<CharacterBase> OnHealthChanged;
        public System.Action<CharacterBase> OnMaxHealthChanged;
        public System.Action<CharacterBase> OnArmorChanged;
        public System.Action<CharacterBase> OnDied;
        public List<Skill> Skills;
        public string Name;

        private int health;
        private int maxHealth;
        private int armor;

        // Effects
        public bool HasActionPriority;
        public int StunDuration;
    }
}