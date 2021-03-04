using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public struct ActionEffectData
    {
        public ActionEffectData(string AlliedText, string EnemyText)
        {
            this.AlliedText = AlliedText;
            this.EnemyText = EnemyText;
        }

        public string AlliedText;
        public string EnemyText;

        public ActionEffectData Swap()
        {
            string temp = AlliedText;
            AlliedText = EnemyText;
            EnemyText = temp;
            return this;
        }
    }

    public class ActionExecutionBehaviour
    {
        public ActionEffectData[] Execute(Action AlliedAction, Action EnemyAction)
        {
            ActionEffectData FirstActionData = Execute_Internal(AlliedAction, EnemyAction);
            ActionEffectData SecondActionData = Execute_Internal(EnemyAction, AlliedAction);

            return new ActionEffectData[2] { FirstActionData, SecondActionData.Swap() };
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (!IsDead(AttackerAction.Owner))
            {
                if (AttackerAction.ActionType == CharacterActionType.Attack)
                {
                    return DoAction_Attack(AttackerAction, DefenderAction);
                }
                if (AttackerAction.ActionType == CharacterActionType.RandomAttack)
                {
                    return DoAction_RandomAttack(AttackerAction, DefenderAction);
                }
                if (AttackerAction.ActionType == CharacterActionType.SelfAttack)
                {
                    return DoAction_SelfAttack(AttackerAction);
                }
                if (AttackerAction.ActionType == CharacterActionType.SelfRandomAttack)
                {
                    return DoAction_SelfRandomAttack(AttackerAction);
                }
            }
            return new ActionEffectData();
        }

        private bool IsDead(CharacterBase Character)
        {
            return Character == null || Character.IsDead;
        }

        private ActionEffectData DoAction_Attack(Action AttackerAction, Action DefenderAction)
        {
            if (!IsDead(DefenderAction.Owner))
            {
                DefenderAction.Owner.Health -= AttackerAction.Value;
                return new ActionEffectData("", (-AttackerAction.Value).ToString());
            }
            return new ActionEffectData();
        }

        private ActionEffectData DoAction_SelfAttack(Action AttackerAction)
        {
            AttackerAction.Owner.Health -= AttackerAction.Value;
            return new ActionEffectData((-AttackerAction.Value).ToString(), "");
        }

        private ActionEffectData DoAction_RandomAttack(Action AttackerAction, Action DefenderAction)
        {
            bool success = UnityEngine.Random.value > 0.4f; // Fair random :)
            if (success)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            return new ActionEffectData("Miss", "");
        }

        private ActionEffectData DoAction_SelfRandomAttack(Action AttackerAction)
        {
            bool success = UnityEngine.Random.value > 0.4f; // Fair random :)
            if (success)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
            return new ActionEffectData("Miss", "");
        }
    }
}