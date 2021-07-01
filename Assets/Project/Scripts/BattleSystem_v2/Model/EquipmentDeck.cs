using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TimelineHero.Character;

namespace TimelineHero.Battle_v2
{
    public class EquipmentDeck
    {
        public EquipmentDeck(string Name)
        {
            SkillName = Name;
            Deck = new Queue<Skill>();
        }

        public EquipmentDeck(string Name, SkillSet InSkillSet)
        {
            SkillName = Name;
            Deck = new Queue<Skill>(InSkillSet.Skills);
        }

        public string SkillName = "Default";

        public Queue<Skill> Deck;
        public List<Skill> Hand = new List<Skill>();

        public System.Action<Skill> OnDrawCard;
        public System.Action<Skill> OnDiscardCard;

        public void Draw()
        {
            Skill card = Deck.Dequeue();
            Hand.Add(card);
            OnDrawCard?.Invoke(card);
        }

        public void Discard(Skill SkillToDiscard)
        {
            Hand.Remove(SkillToDiscard);
            Deck.Enqueue(SkillToDiscard);
            OnDiscardCard?.Invoke(SkillToDiscard);
        }

        public EquipmentDeck Clone()
        {
            EquipmentDeck clone = new EquipmentDeck(SkillName);
            clone.Deck = new Queue<Skill>(Deck.Select(skill => skill.Clone()));
            clone.Hand = Hand.Select(skill => skill.Clone()).ToList();
            return clone;
        }
    }
}