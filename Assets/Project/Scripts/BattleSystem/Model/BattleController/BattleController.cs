using System.Collections.Generic;
using System.Collections;
using TimelineHero.Battle;
using TimelineHero.BattleUI;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.Core;
using UnityEngine.EventSystems;
using UnityEngine;
using TimelineHero.BattleUI_v2;
using TimelineHero.Battle_v2;

namespace TimelineHero.BattleCardsControl
{
    public class BattleController
    {
        protected IHandView HandCached;
        protected BoardView BoardCached;
        private DiscardSectionView DiscardSectionCached;

        private bool IsActive;
        public CardsControlStrategyBase CardsControlStrategy;

        public void Initialize(IHandView HandRef, BoardView BoardRef)
        {
            HandCached = HandRef;
            BoardCached = BoardRef;

            BattleSystem_v2.Get().OnDiscardAllCardsFromTimeline += DiscardAllCardsFromTimeline;
            BattleSystem_v2.Get().OnEnemyChanged += RegenerateBoard;

            CardsControlStrategy = new BattleCardsControlStrategy(HandRef, BoardRef);
        }

        private void DrawCards(List<Skill> DrawnSkills)
        {
            List<CardWrapper> cards = new List<CardWrapper>();

            foreach (Skill skill in DrawnSkills)
            {
                CardWrapper cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().CardWrapperPrefab);
                cardWrapper.WorldPosition = new Vector2(20, 2);
                cardWrapper.SetState(CardState.Hand, skill);

                cardWrapper.OnPointerDownEvent += OnCardPointerDown;
                cardWrapper.OnPointerUpEvent += OnCardPointerUp;
                cardWrapper.OnDragEvent += OnCardDrag;

                cards.Add(cardWrapper);
            }

            BoardCached.StartCoroutine(DrawCardsCoroutine(cards, GameInstance.Get().DelayBetweenCardAnimationsInSeconds));
        }

        private IEnumerator DrawCardsCoroutine(List<CardWrapper> Cards, float Delay)
        {
            foreach (CardWrapper card in Cards)
            {
                HandCached.AddCard(card);

                yield return new WaitForSeconds(Delay);
            }
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

        private void DiscardCardsFromDiscardSection()
        {
            if (!DiscardSectionCached)
                return;

            DiscardCards(DiscardSectionCached.RemoveAll());
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

            BattleHud_v2.Get().UpdateStatuses(GetStatusPosition(BoardCached.AlliedTimeline),
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