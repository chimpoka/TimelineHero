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
        private DiscardDeck DiscardDeckCached;
        private CharacterTimelineView AlliedTimelineCached;

        private bool IsActive;

        public void Initialize(Hand HandRef, DrawDeck DrawDeckRef, DiscardDeck DiscardDeckRef, CharacterTimelineView AlliedTimelineRef)
        {
            HandCached = HandRef;
            DrawDeckCached = DrawDeckRef;
            DiscardDeckCached = DiscardDeckRef;
            AlliedTimelineCached = AlliedTimelineRef;
        }

        public void DrawCards(int Count, float Delay)
        {
            List<CardWrapper> Cards = DrawDeckCached.Draw(Count);

            if (Cards == null)
                return;

            if (Cards.Count < Count)
            {
                DrawDeckCached.Add(DiscardDeckCached.RemoveAllFromDeck());
                Cards.AddRange(DrawDeckCached.Draw(Count - Cards.Count));
            }

            HandCached.StartCoroutine(DrawCardsCoroutine(Cards, Delay));
        }

        private IEnumerator DrawCardsCoroutine(List<CardWrapper> Cards, float Delay)
        {
            foreach (CardWrapper card in Cards)
            {
                card.OnPointerDownEvent += OnCardPointerDown;
                card.OnPointerUpEvent += OnCardPointerUp;
                card.OnBeginDragEvent += OnCardBeginDrag;
                card.OnDragEvent += OnCardDrag;

                HandCached.AddCard(card);

                yield return new WaitForSeconds(Delay);
            }
        }

        public void DiscardCards(float Delay)
        {
            List<CardWrapper> cards = AlliedTimelineCached.RemoveCardsFromTimeline();
            List<Skill> skills = SkillUtils.GetOriginalSkillsFromCards(cards);

            DiscardDeckCached.Add(skills);

            HandCached.StartCoroutine(DiscardCardsCoroutine(cards, Delay));
        }

        private IEnumerator DiscardCardsCoroutine(List<CardWrapper> Cards, float Delay)
        {
            foreach (CardWrapper card in Cards)
            {
                card.DOAnchorPos(new Vector2(-1000, -500), 1.5f).onComplete += card.DestroyUiObject;

                yield return new WaitForSeconds(Delay);
            }
        }

        public void SetActive(bool Active)
        {
            IsActive = Active;
        }

        #region CardEvents
        public void OnCardPointerDown(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            PlayerCard.DOStop();

            PlayerCard.SetParent(HandCached.transform.parent);
            PlayerCard.AnchoredPosition = eventData.position - PlayerCard.Size / 2;

            if (PlayerCard.State == CardState.Hand)
            {
                HandCached.RemoveCard(PlayerCard);
            }
            else if (PlayerCard.State == CardState.BoardPrePlay)
            {
                AlliedTimelineCached.RemoveCard(PlayerCard);
                AlliedTimelineCached.TryInsertInvisibleCard(PlayerCard);
            }
        }

        public void OnCardPointerUp(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            if (!AlliedTimelineCached.IsPositionInsideBounds(eventData.pointerCurrentRaycast.worldPosition) ||
                !AlliedTimelineCached.TryInsertVisibleCard(PlayerCard))
            {
                HandCached.AddCard(PlayerCard);
            }
        }

        public void OnCardBeginDrag(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;
        }

        public void OnCardEndDrag(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;
        }

        public void OnCardDrag(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            PlayerCard.WorldCenterPosition = eventData.pointerCurrentRaycast.worldPosition;
            
            if (AlliedTimelineCached.IsPositionInsideBounds(eventData.pointerCurrentRaycast.worldPosition))
            {
                AlliedTimelineCached.TryInsertInvisibleCard(PlayerCard);
            }
            else
            {
                AlliedTimelineCached.TryRemoveInvisibleCard();
            }
        }
        #endregion CardEvents
    }
}