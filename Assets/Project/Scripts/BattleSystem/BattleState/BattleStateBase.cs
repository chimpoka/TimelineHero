using TimelineHero.BattleUI;
using TimelineHero.Hud;

namespace TimelineHero.Battle
{
    public abstract class BattleStateBase
    {
        public BattleStateBase(BattleSceneController BattleSceneControllerRef)
        {
            BattleSceneControllerCached = BattleSceneControllerRef;
            Hud = (BattleHud)HudBase.Instance;
        }

        protected BattleSceneController BattleSceneControllerCached;
        protected BattleHud Hud;
    }
}