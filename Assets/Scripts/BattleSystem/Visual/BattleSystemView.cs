﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    public class BattleSystemView : MonoBehaviour
    {
        public SkillContainerView BattleSkillContainer;
        public BattleTimelineView BattleTimeline;
        private BattleSystem BattleSystemCached;

        BattleTimelineTimerView TimerView;
        BattleSkillController SkillController;

        private void Awake()
        {
            GameInstance.Instance.CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            BattleTimeline.SetEnemies(BattleSystemCached.GetEnemyCharacters());
            BattleTimeline.GenerateEnemiesTimeline();
            BattleTimeline.GenerateAlliedTimeline();

            SkillController = new BattleSkillController();
            SkillController.CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;
            SkillController.SetAlliedTimeline(BattleTimeline.GetAlliedTimeline());
            SkillController.SetAlliedCharacters(BattleSystemCached.GetAlliedCharacters());
            SkillController.SetSkillContainer(BattleSkillContainer);
            SkillController.SpawnSkills();
        }

        public void SetBattleSystem(BattleSystem NewBattleSystem)
        {
            BattleSystemCached = NewBattleSystem;
        }
    }
}