using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class ActionExecutionController
    {
        public void Execute(Action EnemyAction, Action AlliedAction)
        {
            if (EnemyAction.ActionType == CharacterActionType.Attack)
            {
                AlliedAction.Owner.Health -= EnemyAction.Value;
            }


            if (AlliedAction.ActionType == CharacterActionType.Attack)
            {
                EnemyAction.Owner.Health -= AlliedAction.Value;
            }
        }
    }
}