using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class PlayTimelineBattleState : BattleStateBase
    {
        public PlayTimelineBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSceneControllerRef.Battle.StartBattleTimer();
            BattleSceneControllerRef.Battle.OnTimerFinished += SetNextState;
            Hud.SetPlayState();
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new ConstructTimelineBattleState(BattleSceneControllerCached);
            BattleSceneControllerCached.Battle.OnTimerFinished -= SetNextState;
        }
    }
}
