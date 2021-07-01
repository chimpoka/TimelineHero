using TimelineHero.BattleUI_v2;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle_v2
{
    public class MainBattleState_v2 : BattleStateBase_v2
    {
        public MainBattleState_v2(BattleSceneController_v2 BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleHud_v2.Get().OnPlayBattleButtonEvent += SetPlayState;
            BattleSystem_v2.Get().SetMainBattleState();
            //BattleSceneControllerRef.BattleView.SetBattleCardsControlStrategy();
        }

        private void SetPlayState()
        {
            BattleHud_v2.Get().OnPlayBattleButtonEvent -= SetPlayState;
            BattleSceneControllerCached.BattleState = new PlayBattleState_v2(BattleSceneControllerCached);
        }
    }
}