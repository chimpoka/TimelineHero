namespace TimelineHero.Battle
{
    public abstract class BattleStateBase
    {
        public BattleStateBase(BattleSceneController BattleSceneControllerRef)
        {
            BattleSceneControllerCached = BattleSceneControllerRef;
        }

        protected BattleSceneController BattleSceneControllerCached;
    }
}