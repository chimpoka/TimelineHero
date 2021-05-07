namespace TimelineHero.Battle
{
    public class PlayTimelineBattleState : BattleStateBase
    {
        public PlayTimelineBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            Hud.SetPlayState();

            BattleSystem.Get().OnTimerFinished += SetNextState;
            BattleSystem.Get().InitializePlayState();
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new ConstructTimelineBattleState(BattleSceneControllerCached);
            BattleSystem.Get().OnTimerFinished -= SetNextState;
        }
    }
}
