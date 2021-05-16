using TimelineHero.BattleUI;

namespace TimelineHero.Battle
{
    public class InitialBattleState : BattleStateBase
    {
        public InitialBattleState(BattleSceneController BattleSceneControllerRef)
            : base(BattleSceneControllerRef)
        {
            BattleHud.Get().SetInitialBattleState();
            BattleSystem.Get().SetInitialBattleState();

            BattleSceneControllerCached.BattleState = new MainBattleState(BattleSceneControllerCached);
        }
    }
}