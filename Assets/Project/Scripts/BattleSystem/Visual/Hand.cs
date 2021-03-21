using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class Hand : UiComponent
    {
        // Top, Bottom, Left, Right
        public Vector4 Border = new Vector4(20, 20, 20, 20);
        public Vector2 CardOffset = new Vector2(10, 10);

        private List<CardWrapper> Cards;

        public void AddCard(CardWrapper NewCard)
        {
            Cards = Cards ?? new List<CardWrapper>();

            Cards.Add(NewCard);
            NewCard.SetParent(GetTransform());
            NewCard.State = CardState.Hand;
            ShrinkSkills();
        }

        public void RemoveCard(CardWrapper CardToRemove)
        {
            Cards.Remove(CardToRemove);
            CardToRemove.State = CardState.NoParent;
            ShrinkSkills();
        }

        private void ShrinkSkills()
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