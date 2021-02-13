using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class BattleSystemView
    {
        BattleSystem Battle;
        BattleTimelineView TimelineView;
        BattleTimelineTimerView TimerView;
        SkillContainerView SkillContainer;

        BattleSystemView(BattleSystem Battle)
        {
            this.Battle = Battle;
        }
    }
}