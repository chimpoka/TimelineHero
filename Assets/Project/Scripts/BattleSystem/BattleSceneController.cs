using TimelineHero.Core;
using TimelineHero.BattleView;
using TimelineHero.BattleUI;

namespace TimelineHero.Battle
{
    public class BattleSceneController : SceneControllerBase
    {
        public BattleSystemView BattleView;
        public BattleStateBase BattleState;

        private void Start()
        {
            BattleSystem.Get().OnBattleFinished += OnBattleFinished;
            BattleView.Initialize();

            BattleHud.Get().Initialize();

            BattleState = new InitialBattleState(this);
        }

        private void OnBattleFinished(BattleResult Result)
        {
            if (Result == BattleResult.Lose)
            {
                BattleState = new LoseBattleState(this);
            }
            else if (Result == BattleResult.Win)
            {
                BattleState = new WinBattleState(this);
            }
        }

        protected override void InitializeSubsystems()
        {
            Subsystems.Add(new BattleSystem());
        }
    }
}