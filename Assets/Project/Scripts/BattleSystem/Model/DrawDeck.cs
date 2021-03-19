using System.Collections;
using System.Collections.Generic;
using TimelineHero.Character;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class DrawDeck
    {
        public List<Skill> Skills;

        public System.Action<int> OnDeckSizeChanged;

        public void Initialize(List<Skill> NewSkills)
        {
            Skills = NewSkills;
            //Shuffle();
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public void Add(Skill SkillToAdd)
        {
            Skills.Add(SkillToAdd);
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public void Add(List<Skill> SkillsToAdd)
        {
            Skills.AddRange(SkillsToAdd);
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public List<Card> Draw(int Count)
        {
            if (Skills.Count == 0)
                return new List<Card>();

            if (Skills.Count > 0 && Skills.Count < Count)
            {
                return CreateCardsAndRemoveFromDeck(Skills.Count);
            }
            else if (Skills.Count >= Count)
            {
                return CreateCardsAndRemoveFromDeck(Count);
            }

            return null;
        }

        private List<Card> CreateCardsAndRemoveFromDeck(int Count)
        {
            List<Card> Cards = new List<Card>();

            for (int i = 0; i < Count; ++i)
            {
                Card Card = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
                Card.SetSkill(Skills[0]);
                Card.WorldPosition = new Vector2(20, 2);
                Skills.RemoveAt(0);
                Cards.Add(Card);
            }

            OnDeckSizeChanged?.Invoke(Skills.Count);

            return Cards;
        }

        private void Shuffle()
        {
            int n = Skills.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                Skill value = Skills[k];
                Skills[k] = Skills[n];
                Skills[n] = value;
            }
        }
    }
}