using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class HandView : UiComponent, IHandView
    {
        // Top, Bottom, Left, Right
        public Vector4 Border = new Vector4(20, 20, 20, 20);
        public Vector2 CardOffset = new Vector2(10, 10);

        private List<CardWrapper> Cards = new List<CardWrapper>();

        private List<Skill> GetSkills()
        {
            return Cards.Select(card => card.GetOriginalSkill()).ToList();
        }

        private void UpdateHandData()
        {
            BattleSystem.Get().PlayerHand.SetSkills(GetSkills());
        }

        public Transform GetParent()
        {
            return transform.parent;
        }

        public void AddCard(CardWrapper NewCard)
        {
            Cards.Add(NewCard);
            NewCard.SetState(CardState.Hand, NewCard.GetOriginalSkill());
            NewCard.SetParent(GetTransform());
            ShrinkCards();

            UpdateHandData();
        }

        public void AddCards(List<CardWrapper> NewCards)
        {
            foreach (CardWrapper card in NewCards)
            {
                AddCard(card);
            }
        }

        public void RemoveCard(CardWrapper CardToRemove)
        {
            Cards.Remove(CardToRemove);
            ShrinkCards();

            UpdateHandData();
        }

        private void ShrinkCards()
        {
            Vector2 newPosition = new Vector2(Border.y, Border.z);

            foreach (CardWrapper card in Cards)
            {
                if (newPosition.x + card.Size.x > Size.x)
                {
                    newPosition.x = Border.z;
                    newPosition.y += Card.GetCardStaticHeight() + CardOffset.y;
                }

                card.DOAnchorPos(newPosition);
                
                newPosition += new Vector2(card.Size.x + CardOffset.x, 0);
            }
        }
    }
}