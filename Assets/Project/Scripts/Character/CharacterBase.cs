using System.Collections.Generic;
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

        public List<Skill> Skills { get => SkillSets[0].Skills; }

        public System.Action<CharacterBase> OnHealthChanged;
        public System.Action<CharacterBase> OnMaxHealthChanged;
        public System.Action<CharacterBase> OnBlockChanged;
        public System.Action<CharacterBase> OnAdrenalineChanged;
        public System.Action<CharacterBase> OnDied;
        public List<SkillSet> SkillSets;
        public EquipmentSet CurrentEquipment;
        public Dictionary<string, Skill> SkillsDict;
        public string Name;

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

        public Skill GetSkill(string Name)
        {
            return SkillsDict[Name].Clone();
        }
    }

    public class SkillSet
    {
        public string SkillName;
        public List<Skill> Skills;
    }
    
    public enum EquipmentType { OneHand, TwoHands, Body, Boots, Consumable }
    public enum EquipmentSlot { LeftHand, RightHand, TwoHands, Body, Boots, Consumable }

    public class Equipment
    {
        public Equipment()
        {
        }

        public Equipment(Equipment other)
        {
            Name = other.Name;
            EquipmentIcon = other.EquipmentIcon;
            Type = other.Type;
            Slot = other.Slot;
            EquipmentDecks = other.EquipmentDecks;
        }

        public string Name;
        public Sprite EquipmentIcon;
        public EquipmentType Type;
        public EquipmentSlot Slot;
        public List<Battle.EquipmentDeck> EquipmentDecks;

        public Equipment Clone()
        {
            Equipment clone = new Equipment(this);
            clone.EquipmentDecks = EquipmentDecks.Select(deck => deck.Clone()).ToList();
            return clone;
        }

        public void DiscardAllCards()
        {
            EquipmentDecks.ForEach(x => x.DiscardAll());
        }

        public void DrawCardInEachSlot()
        {
            EquipmentDecks.ForEach(x => x.Draw());
        }
    }

    public class EquipmentSet
    {
        public Equipment LeftHandEquipment;
        public Equipment RightHandEquipment;
        public Equipment TwoHandsEquipment;
        public Equipment BodyEquipment;
        public Equipment BootsEquipnemt;
        public Equipment ConsumableEquipment;

        public List<Equipment> GetAllEquipment()
        {
            return new List<Equipment>(){ LeftHandEquipment, RightHandEquipment, TwoHandsEquipment, BodyEquipment, BootsEquipnemt, ConsumableEquipment };
        }

        public void SetEquipmentInSlot(Equipment NewEquipment, EquipmentSlot Slot)
        {
            NewEquipment.Slot = Slot;
            //GetAllEquipment()[(int)Slot] = NewEquipment;

            switch (Slot)
            {
                case EquipmentSlot.LeftHand: LeftHandEquipment = NewEquipment; break;
                case EquipmentSlot.RightHand: RightHandEquipment = NewEquipment; break;
                case EquipmentSlot.TwoHands: TwoHandsEquipment = NewEquipment; break;
                case EquipmentSlot.Body: BodyEquipment = NewEquipment; break;
                case EquipmentSlot.Boots: BootsEquipnemt = NewEquipment; break;
                case EquipmentSlot.Consumable: ConsumableEquipment = NewEquipment; break;
            }
        }

        public void RefreshDeckAndOpenOneCard()
        {
            DiscardAllCards();
            DrawCardInEachSlot();
        }

        public void DiscardAllCards()
        {
            GetAllEquipment().ForEach(x => x?.DiscardAllCards());
        }

        public void DrawCardInEachSlot()
        {
            GetAllEquipment().ForEach(x => x?.DrawCardInEachSlot());
        }
    }
}