using TimelineHero.BattleView;
using UnityEngine.EventSystems;

namespace TimelineHero.BattleCardsControl
{
    public abstract class CardsControlStrategyBase
    {
        public CardsControlStrategyBase(HandView HandRef, BoardView BoardRef)
        {
            HandCached = HandRef;
            BoardCached = BoardRef;
        }

        protected HandView HandCached;
        protected BoardView BoardCached;

        public abstract void OnCardPointerDown(CardWrapper PlayerCard, PointerEventData eventData);

        public abstract void OnCardPointerUp(CardWrapper PlayerCard, PointerEventData eventData);

        public abstract void OnCardDrag(CardWrapper PlayerCard, PointerEventData eventData);
    }
}