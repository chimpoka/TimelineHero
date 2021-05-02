using UnityEngine;
using TimelineHero.UI;

namespace TimelineHero.Battle
{
    public class LoseBattleState : BattleStateBase
    {
        public LoseBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem.Get().StopBattleTimer();
            WindowManager.Instance.ShowLoseWindow();
        }
    }
}