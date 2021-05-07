namespace TimelineHero.Battle
{
    public class ConstructTimelineBattleState : BattleStateBase
    {
        public ConstructTimelineBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            Hud.OnPlayBattleButtonEvent += SetNextState;
            Hud.SetConstructState();

            BattleSystem.Get().InitializeConstructState();
        }

        private void SetNextState()
        {
            BattleSceneControllerCached.BattleState = new PlayTimelineBattleState(BattleSceneControllerCached);
            Hud.OnPlayBattleButtonEvent -= SetNextState;
        }
    }
}