using TimelineHero.BattleUI_v2;

namespace TimelineHero.Battle_v2
{
    public class PlayBattleState_v2 : BattleStateBase_v2
    {
        public PlayBattleState_v2(BattleSceneController_v2 BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem_v2.Get().OnTimerFinished += SetNextState;
            BattleSystem_v2.Get().SetPlayBattleState();

            BattleHud_v2.Get().SetPlayBattleState();
        }

        private void SetNextState()
        {
            BattleSystem_v2.Get().OnTimerFinished -= SetNextState;
            BattleSceneControllerCached.BattleState = new InitialBattleState_v2(BattleSceneControllerCached);
        }
    }
}
