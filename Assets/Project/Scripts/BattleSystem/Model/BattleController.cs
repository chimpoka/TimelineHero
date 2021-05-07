using System.Collections.Generic;
using System.Collections;
using TimelineHero.Battle;
using TimelineHero.BattleUI;
using TimelineHero.Character;
using TimelineHero.Core;
using TimelineHero.Hud;
using UnityEngine.EventSystems;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class BattleController
    {
        private HandView HandCached;
        private BoardView BoardCached;

        private bool IsActive;

        public void Initialize(HandView HandRef, BoardView BoardRef)
        {
            HandCached = HandRef;
            BoardCached = BoardRef;

            BattleSystem.Get().OnDrawCards += DrawCards;
            BattleSystem.Get().OnDiscardAllCardsFromTimeline += DiscardAllCardsFromTimeline;
            BattleSystem.Get().OnEnemyChanged += RegenerateBoard;
        }

        private void DrawCards(List<Skill> DrawnSkills)
        {
            List<CardWrapper> cards = new List<CardWrapper>();

            foreach (Skill skill in DrawnSkills)
            {
                CardWrapper cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardWrapperPrefab);
                cardWrapper.WorldPosition = new Vector2(20, 2);
                cardWrapper.SetState(CardState.Hand, skill);

                cardWrapper.OnPointerDownEvent += OnCardPointerDown;
                cardWrapper.OnPointerUpEvent += OnCardPointerUp;
                cardWrapper.OnBeginDragEvent += OnCardBeginDrag;
                cardWrapper.OnEndDragEvent += OnCardEndDrag;
                cardWrapper.OnDragEvent += OnCardDrag;

                cards.Add(cardWrapper);
            }

            HandCached.StartCoroutine(DrawCardsCoroutine(cards, GameInstance.Instance.DelayBetweenCardAnimationsInSeconds));
        }

        private IEnumerator DrawCardsCoroutine(List<CardWrapper> Cards, float Delay)
        {
            foreach (CardWrapper card in Cards)
            {
                HandCached.AddCard(card);

                yield return new WaitForSeconds(Delay);
            }
        }

        private void DiscardAllCardsFromTimeline()
        {
            if (!BoardCached.AlliedTimeline)
                return;

            List<CardWrapper> cards = BoardCached.AlliedTimeline.RemoveCardsFromTimeline();
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

            BattleHud hud = (BattleHud)HudBase.Instance;
            hud.UpdateStatuses(GetStatusPosition(BoardCached.AlliedTimeline),
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

            PlayerCard.DOStop();

            PlayerCard.SetParent(HandCached.transform.parent);
            PlayerCard.AnchoredPosition = eventData.position - PlayerCard.Size / 2;

            AlliedCharacterTimelineView alliedTimeline = BoardCached.AlliedTimeline;

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

            AlliedCharacterTimelineView alliedTimeline = BoardCached.AlliedTimeline;

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

            AlliedCharacterTimelineView alliedTimeline = BoardCached.AlliedTimeline;

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