using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public class DiscardCardBattleState : BattleStateBase
    {
        public DiscardCardBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            WindowManager.OnDiscardWindowClosed += SetMainBattleState;

            BattleSceneControllerRef.BattleView.SetDiscardCardsControlStrategy();
        }

        private void SetMainBattleState()
        {
            WindowManager.OnDiscardWindowClosed -= SetMainBattleState;
            BattleSceneControllerCached.BattleState = new MainBattleState(BattleSceneControllerCached);
        }
    }
}