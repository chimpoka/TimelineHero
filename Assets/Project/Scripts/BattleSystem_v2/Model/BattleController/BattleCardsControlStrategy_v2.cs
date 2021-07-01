using TimelineHero.BattleCardsControl;
using TimelineHero.BattleView;
using TimelineHero.BattleView_v2;
using UnityEngine.EventSystems;

namespace TimelineHero.Battle_v2
{
    public class BattleCardsControlStrategy_v2 : BattleCardsControlStrategy
    {
        public BattleCardsControlStrategy_v2(IHandView HandRef, BoardView BoardRef) 
            : base(HandRef, BoardRef)
        {
        }
    }
}