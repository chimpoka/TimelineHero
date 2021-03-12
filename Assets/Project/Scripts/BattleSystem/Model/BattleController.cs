using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
using UnityEngine.EventSystems;
using TimelineHero.Core;
using System.Collections;

namespace TimelineHero.Battle
{
    public class BattleController
    {
        private Hand HandCached;
        private DrawDeck DrawDeckCached;
        private CharacterTimelineView AlliedTimelineCached;

        private bool IsActive;

        public void Initialize(Hand HandRef, DrawDeck DrawDeckRef, CharacterTimelineView AlliedTimelineRef)
        {
            HandCached = HandRef;
            DrawDeckCached = DrawDeckRef;
            AlliedTimelineCached = AlliedTimelineRef;
        }

        public void DrawCards(int Count, float Delay)
        {
            HandCached.StartCoroutine(DrawCardsCoroutine(Count, Delay));
        }

        private IEnumerator DrawCardsCoroutine(int Count, float Delay)
        {
            List<Card> Cards = DrawDeckCached.Draw(Count);

            if (Cards == null)
                yield break;

            foreach (Card card in Cards)
            {
                card.LocationType = CardLocationType.Hand;

                card.OnPointerDownEvent += OnCardPointerDown;
                card.OnPointerUpEvent += OnCardPointerUp;
                card.OnBeginDragEvent += OnCardBeginDrag;
                card.OnDragEvent += OnCardDrag;

                HandCached.AddCard(card);

                yield return new WaitForSeconds(Delay);
            }
        }

        public void SetActive(bool Active)
        {
            IsActive = Active;
        }

        #region CardEvents
        public void OnCardPointerDown(Card PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            PlayerCard.DOStop();

            PlayerCard.AnchoredPosition += new Vector2(-6, 6);

            if (PlayerCard.LocationType == CardLocationType.Hand)
            {
                HandCached.RemoveCard(PlayerCard);
            }
            else if (PlayerCard.LocationType == CardLocationType.Board)
            {
                AlliedTimelineCached.RemoveCard(PlayerCard);
            }

            // Move to foreground
            PlayerCard.GetTransform().SetSiblingIndex(PlayerCard.GetTransform().parent.childCount - 1);
        }

        public void OnCardPointerUp(Card PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            if (AlliedTimelineCached.IsPositionInsideBounds(PlayerCard.WorldBounds.center))
            {
                if (!AlliedTimelineCached.TryAddSkill(PlayerCard))
                {
                    HandCached.AddCard(PlayerCard);
                }
            }
            else
            {
                HandCached.AddCard(PlayerCard);
            }
        }

        public void OnCardBeginDrag(Card PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;
        }

        public void OnCardEndDrag(Card PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;
        }

        public void OnCardDrag(Card PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            PlayerCard.AnchoredPosition += eventData.delta / GameInstance.Instance.CanvasScaleFactor;
        }
        #endregion CardEvents
    }
}