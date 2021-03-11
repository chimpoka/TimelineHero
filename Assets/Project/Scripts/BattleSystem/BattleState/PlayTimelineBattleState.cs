namespace TimelineHero.Battle
{
    public class PlayTimelineBattleState : BattleStateBase
    {
        public PlayTimelineBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleSceneControllerRef.Battle.OnTimerFinished += SetNextState;
            BattleSceneControllerRef.Battle.StartBattleTimer();
            BattleSceneControllerRef.BattleView.SetActive(false);
            BattleSceneControllerRef.BattleView.BattleTimeline.RebuildSkillsForPlayState();
            Hud.SetPlayState();
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new ConstructTimelineBattleState(BattleSceneControllerCached);
            BattleSceneControllerCached.Battle.OnTimerFinished -= SetNextState;
        }
    }
}
