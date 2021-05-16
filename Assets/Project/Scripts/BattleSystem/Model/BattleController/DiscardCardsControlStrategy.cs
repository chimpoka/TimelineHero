using TimelineHero.BattleView;
using UnityEngine.EventSystems;

namespace TimelineHero.BattleCardsControl
{
    public class DiscardCardsControlStrategy : CardsControlStrategyBase
    {
        private DiscardSectionView DiscardSectionCached;

        public DiscardCardsControlStrategy(HandView HandRef, BoardView BoardRef, DiscardSectionView DiscardSectionRef)
            : base(HandRef, BoardRef)
        {
            DiscardSectionCached = DiscardSectionRef;
        }

        public override void OnCardPointerDown(CardWrapper PlayerCard, PointerEventData eventData)
        {
            PlayerCard.DOStop();

            PlayerCard.SetParent(HandCached.transform.parent);

            if (PlayerCard.State == CardState.Hand)
            {
                HandCached.RemoveCard(PlayerCard);
                DiscardSectionCached.AddCard(PlayerCard);
            }
            else if (PlayerCard.State == CardState.Discard)
            {
                DiscardSectionCached.RemoveCard(PlayerCard);
                HandCached.AddCard(PlayerCard);
            }
        }

        public override void OnCardPointerUp(CardWrapper PlayerCard, PointerEventData eventData)
        {
            
        }

        public override void OnCardDrag(CardWrapper PlayerCard, PointerEventData eventData)
        {
            
        }
    }
}