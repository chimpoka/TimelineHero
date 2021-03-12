using System.Collections;
using System.Collections.Generic;
using TimelineHero.Character;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class DrawDeck
    {
        public List<Skill> Skills;

        public void Shuffle()
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

        public void Initialize(List<Skill> NewSkills)
        {
            Skills = NewSkills;
            Shuffle();
        }

        public List<Card> Draw(int Count)
        {
            if (Skills.Count == 0)
                return null;

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

            return Cards;
        }
    }
}