using TimelineHero.BattleView;
using UnityEngine.EventSystems;

namespace TimelineHero.BattleCardsControl
{
    public class BattleCardsControlStrategy : CardsControlStrategyBase
    {
        public BattleCardsControlStrategy(IHandView HandRef, BoardView BoardRef) 
            : base(HandRef, BoardRef)
        {
        }

        public override void OnCardPointerDown(CardWrapper PlayerCard, PointerEventData eventData)
        {
            PlayerCard.DOStop();

            PlayerCard.SetParent(HandCached.GetParent());
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

        public override void OnCardPointerUp(CardWrapper PlayerCard, PointerEventData eventData)
        {
            AlliedCharacterTimelineView alliedTimeline = BoardCached.AlliedTimeline;

            if (!alliedTimeline.IsPositionInsideBounds(eventData.pointerCurrentRaycast.worldPosition) ||
                !alliedTimeline.TryInsertVisibleCard(PlayerCard))
            {
                alliedTimeline.DestroyInvisibleCard();
                HandCached.AddCard(PlayerCard);
            }
        }

        public override void OnCardDrag(CardWrapper PlayerCard, PointerEventData eventData)
        {
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
    }
}