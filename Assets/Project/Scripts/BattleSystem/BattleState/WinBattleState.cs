using UnityEngine;
using TimelineHero.UI;

namespace TimelineHero.Battle
{
    public class WinBattleState : BattleStateBase
    {
        public WinBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem.Get().StopBattleTimer();
            WindowManager.Instance.ShowWinWindow();
        }
    }
}
