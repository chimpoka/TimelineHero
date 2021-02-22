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

        private void Awake()
        {
            Battle = new BattleSystem();
            BattleView.SetBattleSystem(Battle);
        }

        private void Start()
        {
            BattleState = new ConstructTimelineBattleState(this);
        }
    }
}