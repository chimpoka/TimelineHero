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

        public int Block
        {
            get
            {
                return block;
            }
            set
            {
                block = value;
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
        private int block;

        // Effects
        public bool HasActionPriority;
        public int StunDuration;
        public int BlockDuration;
        public int DodgeDuration;
        public int ParryDuration;

        public int Hit(int Damage)
        {
            if (block >= Damage)
            {
                return 0;
            }

            int ActualDamage = Damage - Block;
            Health -= ActualDamage;
            return ActualDamage;
        }
    }
}