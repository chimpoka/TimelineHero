using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class BattleSystemView : MonoBehaviour
    {
        public SkillContainerView BattleSkillContainer;
        public  BattleSystem BattleSystemCached;

        BattleTimelineView TimelineView;
        BattleTimelineTimerView TimerView;
        BattleSkillController SkillController;

        private void Start()
        {
            SkillController = new BattleSkillController();
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