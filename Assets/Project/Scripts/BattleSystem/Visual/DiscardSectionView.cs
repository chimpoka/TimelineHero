using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class DiscardSectionView : UiComponent
    {
        public List<CardWrapper> Cards;

        private float CardsHorizontalOffset = 20.0f;

        private List<Skill> GetSkills()
        {
            return Cards.Select(card => card.GetOriginalSkill()).ToList();
        }

        private void UpdateData()
        {
            BattleSystem.Get().PlayerDiscardSection.SetSkills(GetSkills());
        }

        public void AddCard(CardWrapper NewCard)
        {
            Cards.Add(NewCard);
            NewCard.SetState(CardState.Discard, NewCard.GetOriginalSkill());
            NewCard.SetParent(GetTransform());
            ShrinkCards();

            UpdateData();
        }

        public void RemoveCard(CardWrapper CardToRemove)
        {
            Cards.Remove(CardToRemove);
            ShrinkCards();

            UpdateData();
        }

        public List<CardWrapper> RemoveAll()
        {
            List<CardWrapper> result = new List<CardWrapper>(Cards);
            Cards.Clear();

            UpdateData();

            return result;
        }

        private void ShrinkCards()
        {
            float fullLength = 0.0f;
            foreach (CardWrapper card in Cards)
            {
                fullLength += card.Size.x + CardsHorizontalOffset;
            }
            fullLength -= CardsHorizontalOffset;

            Vector2 discardPosition = new Vector2(Size.x / 2.0f, 0.0f);
            Vector2 startPosition = discardPosition - new Vector2(fullLength / 2.0f, 0.0f);

            foreach (CardWrapper card in Cards)
            {
                card.DOAnchorPos(startPosition);
                startPosition += new Vector2(card.Size.x + CardsHorizontalOffset, 0.0f);
            }
        }
    }
}