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

            if (BattleSceneControllerRef.BattleView.BattleBoard.GetAlliedTimeline())
            {
                BattleSceneControllerRef.BattleView.PlayerBattleController.DiscardCards();
            }

            BattleSceneControllerRef.BattleView.SetActive(true);
            BattleSceneControllerRef.BattleView.BattleBoard.OnStartConstructState();
            BattleSceneControllerRef.BattleView.PlayerBattleController.DrawCards(GameInstance.Instance.DrawCardCount);
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new PlayTimelineBattleState(BattleSceneControllerCached);
            Hud.OnPlayBattleButtonEvent -= SetNextState;
        }
    }
}