using UnityEngine;
using TimelineHero.Hud;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    public class BattleSceneController : SceneControllerBase
    {
        [SerializeField]
        public BattleSystemView BattleView;
        public BattleSystem Battle;
        public BattleStateBase BattleState;

        private void Start()
        {
            Battle = new BattleSystem();
            Battle.OnBattleFinished += OnBattleFinished;
            BattleView.Initialize(Battle);

            BattleHud Hud = (BattleHud)HudBase.Instance;
            Hud.SetBattleSceneController(this);

            BattleState = new ConstructTimelineBattleState(this);
        }

        private void OnBattleFinished(BattleResult Result)
        {
            if (Result == BattleResult.Lost)
            {
                BattleState = new LoseBattleState(this);
            }
            else if (Result == BattleResult.Won)
            {
                BattleState = new WinBattleState(this);
            }
        }
    }
}