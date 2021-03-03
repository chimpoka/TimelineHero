using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class ActionExecutionBehaviour
    {
        public void Execute(Action AlliedAction, Action EnemyAction)
        {
            Execute_Internal(AlliedAction, EnemyAction);
            Execute_Internal(EnemyAction, AlliedAction);
        }

        private void Execute_Internal(Action FirstAction, Action SecondAction)
        {
            if (!IsDead(FirstAction.Owner))
            {
                if (FirstAction.ActionType == CharacterActionType.Attack)
                {
                    DoAction_Attack(FirstAction, SecondAction);
                }
                if (FirstAction.ActionType == CharacterActionType.RandomAttack)
                {
                    DoAction_RandomAttack(FirstAction, SecondAction);
                }
                if (FirstAction.ActionType == CharacterActionType.SelfAttack)
                {
                    DoAction_SelfAttack(FirstAction);
                }
                if (FirstAction.ActionType == CharacterActionType.SelfRandomAttack)
                {
                    DoAction_SelfRandomAttack(FirstAction);
                }
            }
        }

        private bool IsDead(CharacterBase Character)
        {
            return Character == null || Character.IsDead;
        }

        private void DoAction_Attack(Action AttackerAction, Action DefenderAction)
        {
            if (!IsDead(DefenderAction.Owner))
            {
                DefenderAction.Owner.Health -= AttackerAction.Value;
            }
        }

        private void DoAction_SelfAttack(Action AttackerAction)
        {
            AttackerAction.Owner.Health -= AttackerAction.Value;
        }

        private void DoAction_RandomAttack(Action AttackerAction, Action DefenderAction)
        {
            bool success = UnityEngine.Random.value > 0.4f; // Fair random :)
            if (success)
            {
                DoAction_Attack(AttackerAction, DefenderAction);
            }
        }

        private void DoAction_SelfRandomAttack(Action AttackerAction)
        {
            bool success = UnityEngine.Random.value > 0.4f; // Fair random :)
            if (success)
            {
                DoAction_SelfAttack(AttackerAction);
            }
        }
    }
}