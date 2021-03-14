using System.Collections;
using System.Collections.Generic;
using TimelineHero.Core;
using TimelineHero.Hud;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class ConstructTimelineBattleState : BattleStateBase
    {
        public ConstructTimelineBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            Hud.OnPlayBattleButtonEvent += SetNextState;
            Hud.SetConstructState();
            BattleSceneControllerRef.BattleView.SetActive(true);
            BattleSceneControllerRef.BattleView.GetTimerView().ResetPosition();

            GameInstance gi = GameInstance.Instance;
            BattleSceneControllerRef.BattleView.PlayerBattleController.DiscardCards(gi.DelayBetweenCardAnimationsInSeconds);
            BattleSceneControllerRef.BattleView.PlayerBattleController.DrawCards(gi.DrawCardCount, gi.DelayBetweenCardAnimationsInSeconds);
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new PlayTimelineBattleState(BattleSceneControllerCached);
            Hud.OnPlayBattleButtonEvent -= SetNextState;
        }
    }
}