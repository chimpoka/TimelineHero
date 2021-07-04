using System.Collections.Generic;
using System.Linq;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class EquipmentDeck
    {
        public EquipmentDeck(string Name)
        {
            SkillName = Name;
        }

        public EquipmentDeck(SkillSet InSkillSet)
        {
            SkillName = InSkillSet.SkillName;
            Deck = new Queue<Skill>(InSkillSet.Skills);
        }

        public string SkillName = "Default";

        public Queue<Skill> Deck;
        public List<Skill> Hand = new List<Skill>();

        public System.Action<Skill> OnDrawCard;
        public System.Action<Skill> OnDiscardCard;
        public System.Action OnDiscardAllCards;

        public void Draw()
        {
            if (Deck.Count == 0)
                return;

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

        public void DiscardAll()
        {
            //Hand.ForEach(x => Discard(x));
            var cardsToDiscard = new List<Skill>(Hand);
            Hand.Clear();
            cardsToDiscard.ForEach(x => { Deck.Enqueue(x); });
            OnDiscardAllCards?.Invoke();
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