using System.Collections;
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

        BattleSkillController SkillController;

        public void Initialize(BattleSystem BattleSystemRef)
        {
            BattleSystemCached = BattleSystemRef;

            GameInstance.Instance.CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            BattleTimeline.Initialize(BattleSystemCached);

            SkillController = new BattleSkillController();
            SkillController.SetAlliedTimeline(BattleTimeline.GetAlliedTimeline());
            SkillController.SetAlliedCharacters(BattleSystemCached.GetAlliedCharacters());
            SkillController.SetSkillContainer(BattleSkillContainer);
            SkillController.SpawnSkills();
        }

        public BattleTimelineTimerView GetTimerView()
        {
            return BattleTimeline.GetTimerView();
        }

        public void SetActive(bool Active)
        {
            SkillController.SetActive(Active);
        }
    }
}