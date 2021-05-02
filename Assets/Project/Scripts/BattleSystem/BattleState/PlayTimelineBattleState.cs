namespace TimelineHero.Battle
{
    public class PlayTimelineBattleState : BattleStateBase
    {
        public PlayTimelineBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSystem.Get().OnTimerFinished += SetNextState;
            BattleSystem.Get().StartBattleTimer();
            BattleSceneControllerRef.BattleView.SetActive(false);
            BattleSceneControllerRef.BattleView.BattleBoard.OnStartPlayState();
            Hud.SetPlayState();
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new ConstructTimelineBattleState(BattleSceneControllerCached);
            BattleSystem.Get().OnTimerFinished -= SetNextState;
        }
    }
}
