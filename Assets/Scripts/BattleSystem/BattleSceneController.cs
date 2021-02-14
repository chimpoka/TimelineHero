using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Battle;

namespace TimelineHero.Core
{
    public class BattleSceneController : SceneControllerBase
    {
        public  BattleSystem Battle;
        public BattleSystemView BattleView;

        private void Awake()
        {
            Battle = new BattleSystem();
            BattleView.SetBattleSystem(Battle);
        }

        private void Start()
        {

        }
    }
}