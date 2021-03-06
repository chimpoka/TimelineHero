﻿using System.Collections.Generic;
using System.Linq;
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
                OnBlockChanged?.Invoke(this);
            }
        }

        public int Adrenaline
        {
            get
            {
                return adrenaline;
            }
            set
            {
                adrenaline = value;
                OnAdrenalineChanged?.Invoke(this);
            }
        }

        public bool IsDead
        {
            get
            {
                return health <= 0;
            }
        }

        public string Name;
        public System.Action<CharacterBase> OnHealthChanged;
        public System.Action<CharacterBase> OnMaxHealthChanged;
        public System.Action<CharacterBase> OnBlockChanged;
        public System.Action<CharacterBase> OnAdrenalineChanged;
        public System.Action<CharacterBase> OnDied;

        private int health;
        private int maxHealth;
        private int block;
        private int adrenaline;

        // Effects
        public bool HasActionPriority;
        public int StunDuration;
        public int BlockDuration;
        public int DodgeDuration;
        public int ParryDuration;

        public int TakeDamage(int Damage)
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

    public class AlliedCharacter : CharacterBase
    {
        public EquipmentSet CurrentEquipment;
    }

    public class EnemyCharacter : CharacterBase
    {
        public List<SkillSet> SkillSets;
    }

    public class SkillSet
    {
        public string SkillName;
        public List<Skill> Skills;
    }
}