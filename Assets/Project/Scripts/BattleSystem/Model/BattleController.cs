using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
using UnityEngine.EventSystems;
using System.Collections;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    public class BattleController
    {
        private Hand HandCached;
        private DrawDeck DrawDeckCached;
        private DiscardDeck DiscardDeckCached;
        private Board BoardCached;

        private bool IsActive;

        public void Initialize(Hand HandRef, DrawDeck DrawDeckRef, DiscardDeck DiscardDeckRef, Board BoardRef)
        {
            HandCached = HandRef;
            DrawDeckCached = DrawDeckRef;
            DiscardDeckCached = DiscardDeckRef;
            BoardCached = BoardRef;
        }

        public void DrawCards(int Count)
        {
            List<CardWrapper> Cards = DrawDeckCached.Draw(Count);

            if (Cards == null)
                return;

            if (Cards.Count < Count)
            {
                DrawDeckCached.Add(DiscardDeckCached.RemoveAllFromDeck());
                Cards.AddRange(DrawDeckCached.Draw(Count - Cards.Count));
            }

            HandCached.StartCoroutine(DrawCardsCoroutine(Cards, GameInstance.Instance.DelayBetweenCardAnimationsInSeconds));
        }

        private IEnumerator DrawCardsCoroutine(List<CardWrapper> Cards, float Delay)
        {
            foreach (CardWrapper card in Cards)
            {
                card.OnPointerDownEvent += OnCardPointerDown;
                card.OnPointerUpEvent += OnCardPointerUp;
                card.OnBeginDragEvent += OnCardBeginDrag;
                card.OnEndDragEvent += OnCardEndDrag;
                card.OnDragEvent += OnCardDrag;

                HandCached.AddCard(card);

                yield return new WaitForSeconds(Delay);
            }
        }

        public void DiscardCards()
        {
            List<CardWrapper> cards = BoardCached.GetAlliedTimeline().RemoveCardsFromTimeline();
            List<Skill> skills = SkillUtils.GetOriginalSkillsFromCards(cards);

            DiscardDeckCached.Add(skills);

            HandCached.StartCoroutine(DiscardCardsCoroutine(cards, GameInstance.Instance.DelayBetweenCardAnimationsInSeconds));
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

            AlliedCharacterTimeline alliedTimeline = BoardCached.GetAlliedTimeline();

            alliedTimeline.CreateInvisibleCard(PlayerCard);

            if (PlayerCard.State == CardState.Hand)
            {
                HandCached.RemoveCard(PlayerCard);
            }
            else if (PlayerCard.State == CardState.BoardPrePlay)
            {
                alliedTimeline.RemoveCard(PlayerCard);
                alliedTimeline.TryInsertInvisibleCard(PlayerCard);
            }
        }

        public void OnCardPointerUp(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            AlliedCharacterTimeline alliedTimeline = BoardCached.GetAlliedTimeline();

            if (!alliedTimeline.IsPositionInsideBounds(eventData.pointerCurrentRaycast.worldPosition) ||
                !alliedTimeline.TryInsertVisibleCard(PlayerCard))
            {
                alliedTimeline.DestroyInvisibleCard();
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

            AlliedCharacterTimeline alliedTimeline = BoardCached.GetAlliedTimeline();

            if (alliedTimeline.IsPositionInsideBounds(eventData.pointerCurrentRaycast.worldPosition))
            {
                alliedTimeline.TryInsertInvisibleCard(PlayerCard);
            }
            else
            {
                alliedTimeline.RemoveInvisibleCard();
            }
        }
        #endregion CardEvents
    }
}