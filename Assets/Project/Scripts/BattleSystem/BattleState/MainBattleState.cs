using TimelineHero.BattleUI;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public class MainBattleState : BattleStateBase
    {
        public MainBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleHud.Get().OnPlayBattleButtonEvent += SetPlayState;
            WindowManager.Instance.OnDiscardWindowOpened += SetDiscardState;

            BattleSystem.Get().SetMainBattleState();

            BattleSceneControllerRef.BattleView.SetBattleCardsControlStrategy();
        }

        private void UnsubscribeAll()
        {
            BattleHud.Get().OnPlayBattleButtonEvent -= SetPlayState;
            WindowManager.Instance.OnDiscardWindowOpened -= SetDiscardState;
        }

        private void SetPlayState()
        {
            UnsubscribeAll();
            BattleSceneControllerCached.BattleState = new PlayBattleState(BattleSceneControllerCached);
        }

        private void SetDiscardState()
        {
            UnsubscribeAll();
            BattleSceneControllerCached.BattleState = new DiscardCardBattleState(BattleSceneControllerCached);
        }
    }
}