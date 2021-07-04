using System.Collections.Generic;
using System.Collections;
using TimelineHero.Battle;
using TimelineHero.BattleUI;
using TimelineHero.BattleView;
using TimelineHero.Core;
using UnityEngine.EventSystems;
using UnityEngine;

namespace TimelineHero.BattleCardsControl
{
    public class BattleController
    {
        protected IHandView HandCached;
        protected BoardView BoardCached;

        private bool IsActive;
        public CardsControlStrategyBase CardsControlStrategy;

        public void Initialize(IHandView HandRef, BoardView BoardRef)
        {
            HandCached = HandRef;
            BoardCached = BoardRef;

            BattleSystem.Get().OnDiscardAllCardsFromTimeline += DiscardAllCardsFromTimeline;
            BattleSystem.Get().OnEnemyChanged += RegenerateBoard;

            CardsControlStrategy = new BattleCardsControlStrategy(HandRef, BoardRef);
        }

        private void DiscardCards(List<CardWrapper> Cards)
        {
            if (Cards == null || Cards.Count == 0)
                return;

            BoardCached.StartCoroutine(DiscardCardsCoroutine(Cards, GameInstance.Get().DelayBetweenCardAnimationsInSeconds));
        }

        private IEnumerator DiscardCardsCoroutine(List<CardWrapper> Cards, float Delay)
        {
            foreach (CardWrapper card in Cards)
            {
                card.DOAnchorPos(new Vector2(-1000, -500), 1.5f).onComplete += card.DestroyUiObject;

                yield return new WaitForSeconds(Delay);
            }
        }

        protected void DiscardAllCardsFromTimeline()
        {
            if (!BoardCached.AlliedTimeline)
                return;

            DiscardCards(BoardCached.AlliedTimeline.RemoveCardsFromTimeline());
        }

        public void SetActive(bool Active)
        {
            IsActive = Active;
        }

        public void RegenerateBoard()
        {
            List<CardWrapper> alliedCards = null;
            alliedCards = BoardCached.AlliedTimeline?.RemoveCardsFromTimeline();

            BoardCached.RegenerateTimelinesAndTimerView();

            if (alliedCards != null)
            {
                foreach (CardWrapper card in alliedCards)
                {
                    if (!BoardCached.AlliedTimeline.TryAddCard(card))
                    {
                        HandCached.AddCard(card);
                    }
                }
            }

            BattleHud.Get().UpdateStatuses(GetStatusPosition(BoardCached.AlliedTimeline),
                                           GetStatusPosition(BoardCached.EnemyTimeline));
        }

        private Vector2 GetStatusPosition(CharacterTimelineView TimelineView)
        {
            Bounds bounds = TimelineView.WorldBounds;
            return new Vector2(bounds.max.x, bounds.center.y);
        }

        #region CardEvents
        public void OnCardPointerDown(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            CardsControlStrategy.OnCardPointerDown(PlayerCard, eventData);
        }

        public void OnCardPointerUp(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            CardsControlStrategy.OnCardPointerUp(PlayerCard, eventData);
        }

        public void OnCardDrag(CardWrapper PlayerCard, PointerEventData eventData)
        {
            if (!IsActive)
                return;

            CardsControlStrategy.OnCardDrag(PlayerCard, eventData);
        }
        #endregion CardEvents
    }
}