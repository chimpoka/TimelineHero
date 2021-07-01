using TimelineHero.BattleUI_v2;

namespace TimelineHero.Battle_v2
{
    public class InitialBattleState_v2 : BattleStateBase_v2
    {
        public InitialBattleState_v2(BattleSceneController_v2 BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleHud_v2.Get().SetInitialBattleState();
            BattleSystem_v2.Get().SetInitialBattleState();

            BattleSceneControllerCached.BattleState = new MainBattleState_v2(BattleSceneControllerCached);
        }
    }
}