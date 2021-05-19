using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public class DiscardCardBattleState : BattleStateBase
    {
        public DiscardCardBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            WindowManager.Get().OnDiscardWindowClosed += SetMainBattleState;

            BattleSceneControllerRef.BattleView.SetDiscardCardsControlStrategy();
        }

        private void SetMainBattleState()
        {
            WindowManager.Get().OnDiscardWindowClosed -= SetMainBattleState;
            BattleSceneControllerCached.BattleState = new MainBattleState(BattleSceneControllerCached);
        }
    }
}