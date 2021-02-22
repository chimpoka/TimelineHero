using System.Collections;
using System.Collections.Generic;
using TimelineHero.Core;
using TimelineHero.Hud;
using UnityEngine;

namespace TimelineHero.Battle
{
    public abstract class BattleStateBase : System.IDisposable
    {
        public BattleStateBase(BattleSceneController BattleSceneControllerRef)
        {
            BattleSceneControllerCached = BattleSceneControllerRef;
            Hud = (BattleHud)HudBase.Instance;
        }

        protected BattleSceneController BattleSceneControllerCached;
        protected BattleHud Hud;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}