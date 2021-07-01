namespace TimelineHero.Battle_v2
{
    public abstract class BattleStateBase_v2
    {
        public BattleStateBase_v2(BattleSceneController_v2 BattleSceneControllerRef)
        {
            BattleSceneControllerCached = BattleSceneControllerRef;
        }

        protected BattleSceneController_v2 BattleSceneControllerCached;
    }
}