using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Battle;

namespace TimelineHero.Core
{
    public class BattleSceneController : SceneControllerBase
    {
        BattleSystem Battle;
        BattleSystemView BattleView;

        private void Start()
        {
            Battle = new BattleSystem();
        }
    }
}