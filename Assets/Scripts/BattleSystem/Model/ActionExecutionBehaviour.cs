using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class ActionExecutionBehaviour
    {
        public void Execute(Action AlliedAction, Action EnemyAction)
        {
            if (AlliedAction.ActionType == CharacterActionType.Attack)
            {
                EnemyAction.Owner.Health -= AlliedAction.Value;
            }


            if (EnemyAction.ActionType == CharacterActionType.Attack)
            {
                if (AlliedAction.Owner != null) // think about it
                {
                    AlliedAction.Owner.Health -= EnemyAction.Value;
                }
            }
        }
    }
}