using TimelineHero.Battle;
using TimelineHero.BattleUI_v2;
using TimelineHero.BattleView_v2;
using TimelineHero.Core;

namespace TimelineHero.Battle_v2
{
    public class BattleSceneController_v2 : SceneControllerBase
    {
        public BattleSystemView_v2 BattleView;
        public BattleStateBase_v2 BattleState;

        private void Start()
        {
            BattleSystem_v2.Get().OnBattleFinished += OnBattleFinished;
            BattleView.Initialize();

            BattleHud_v2.Get().Initialize();

            BattleState = new InitialBattleState_v2(this);
        }

        private void OnBattleFinished(BattleResult Result)
        {
            if (Result == BattleResult.Lose)
            {
                BattleState = new LoseBattleState_v2(this);
            }
            else if (Result == BattleResult.Win)
            {
                BattleState = new WinBattleState_v2(this);
            }
        }

        protected override void InitializeSubsystems()
        {
            Subsystems.Add(new BattleSystem_v2());
        }
    }
}