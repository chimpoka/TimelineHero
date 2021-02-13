using System.Collections;
using System.Collections.Generic;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class BattleSystem : MonoBehaviour
    {
        public GameInstance Game;

        private BattleStateBase BattleState;
        public BattleTimelineTimer TimelineTimer;
    }
}