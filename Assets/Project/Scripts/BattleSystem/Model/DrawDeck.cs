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

        public List<CardWrapper> Draw(int Count)
        {
            if (Skills.Count == 0)
                return new List<CardWrapper>();

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

        private List<CardWrapper> CreateCardsAndRemoveFromDeck(int Count)
        {
            List<CardWrapper> Cards = new List<CardWrapper>();

            for (int i = 0; i < Count; ++i)
            {
                CardWrapper cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardWrapperPrefab);
                cardWrapper.WorldPosition = new Vector2(20, 2);

                Card card = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardPrefab);
                card.SetSkill(Skills[0]);
                Skills.RemoveAt(0);

                cardWrapper.SetState(CardState.NoParent, card);
                Cards.Add(cardWrapper);
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