using TimelineHero.BattleUI;

namespace TimelineHero.Battle
{
    public class MainBattleState : BattleStateBase
    {
        public MainBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleHud.Get().OnPlayBattleButtonEvent += SetPlayState;
            BattleSystem.Get().SetMainBattleState();
        }

        private void SetPlayState()
        {
            BattleHud.Get().OnPlayBattleButtonEvent -= SetPlayState;
            BattleSceneControllerCached.BattleState = new PlayBattleState(BattleSceneControllerCached);
        }
    }
}