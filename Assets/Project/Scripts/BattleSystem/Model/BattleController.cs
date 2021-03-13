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
            List<Card> Cards = DrawDeckCached.Draw(Count);

            if (Cards == null)
                return;

            if (Cards.Count < Count)
            {
                DrawDeckCached.Add(DiscardDeckCached.RemoveAllFromDeck());
                Cards.AddRange(DrawDeckCached.Draw(Count - Cards.Count));
            }

            HandCached.StartCoroutine(DrawCardsCoroutine(Cards, Delay));
        }

        private IEnumerator DrawCardsCoroutine(List<Card> Cards, float Delay)
        {
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

        public void DiscardCards(float Delay)
        {
            List<Card> cards = AlliedTimelineCached.RemoveCardsFromTimeline();
            List<Skill> skills = SkillUtils.GetSkillsFromCardsList(cards);

            DiscardDeckCached.Add(skills);

            HandCached.StartCoroutine(DiscardCardsCoroutine(cards, Delay));
        }

        private IEnumerator DiscardCardsCoroutine(List<Card> Cards, float Delay)
        {
            foreach (Card card in Cards)
            {
                card.DOAnchorPos(new Vector2(-1000, -500), 1.5f).onComplete += card.DestroyGameObject;

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
                if (!AlliedTimelineCached.TryAddCard(PlayerCard))
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