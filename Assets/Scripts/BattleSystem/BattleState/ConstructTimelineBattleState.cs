﻿using System.Collections;
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
            BattleSceneControllerRef.BattleView.SetActive(true);
            BattleSceneControllerRef.BattleView.GetTimerView().ResetPosition();
            BattleSceneControllerRef.BattleView.BattleTimeline.ClearAlliedTimeline();
            Hud.SetConstructState();
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new PlayTimelineBattleState(BattleSceneControllerCached);
            Hud.OnPlayBattleButtonEvent -= SetNextState;
        }
    }
}