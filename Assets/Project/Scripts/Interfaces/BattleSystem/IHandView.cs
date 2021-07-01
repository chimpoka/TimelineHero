using UnityEngine;

namespace TimelineHero.BattleView
{
    public interface IHandView
    {
        public Transform GetParent();
        public void AddCard(CardWrapper CardWrapperRef);
        public void RemoveCard(CardWrapper CardWrapperRef);
    }
}