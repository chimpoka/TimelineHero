using TimelineHero.BattleUI;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public class LoseBattleState : BattleStateBase
    {
        public LoseBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem.Get().StopBattleTimer();
            BattleHud.Get().OpenWindow<LoseWindow>();
        }
    }
}