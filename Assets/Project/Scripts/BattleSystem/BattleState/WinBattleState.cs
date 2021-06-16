using TimelineHero.BattleUI;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public class WinBattleState : BattleStateBase
    {
        public WinBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem.Get().StopBattleTimer();
            BattleHud.Get().OpenWindow<WinWindow>();
        }
    }
}
