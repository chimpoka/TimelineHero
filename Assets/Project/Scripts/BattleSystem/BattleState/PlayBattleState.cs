using TimelineHero.BattleUI;

namespace TimelineHero.Battle
{
    public class PlayBattleState : BattleStateBase
    {
        public PlayBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem.Get().OnTimerFinished += SetNextState;
            BattleSystem.Get().SetPlayBattleState();

            BattleHud.Get().SetPlayBattleState();
        }

        private void SetNextState()
        {
            BattleSystem.Get().OnTimerFinished -= SetNextState;
            BattleSceneControllerCached.BattleState = new InitialBattleState(BattleSceneControllerCached);
        }
    }
}
