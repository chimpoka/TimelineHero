using TimelineHero.BattleUI;
using TimelineHero.BattleUI_v2;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle_v2
{
    public class LoseBattleState_v2 : BattleStateBase_v2
    {
        public LoseBattleState_v2(BattleSceneController_v2 BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem_v2.Get().StopBattleTimer();
            BattleHud_v2.Get().OpenWindow<LoseWindow>();
        }
    }
}