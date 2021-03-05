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

        public bool IsValid()
        {
            return AlliedText != null && EnemyText != null;
        }
    }

    public class ActionExecutionBehaviour
    {
        public ActionEffectData[] Execute(Action AlliedAction, Action EnemyAction)
        {
            if (IsDead(AlliedAction.Owner) || IsDead(EnemyAction.Owner))
            {
                return null;
            }

            ActionEffectData FirstStunActionData = TryDecreaseStunDuration(AlliedAction.Owner);
            ActionEffectData SecondStunActionData = TryDecreaseStunDuration(EnemyAction.Owner);

            ActionEffectData FirstActionData = Execute_Internal(AlliedAction, EnemyAction);
            ActionEffectData SecondActionData = Execute_Internal(EnemyAction, AlliedAction);

            return new ActionEffectData[4] { FirstStunActionData, SecondStunActionData.Swap(), FirstActionData, SecondActionData.Swap() };
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
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
            if (AttackerAction.ActionType == CharacterActionType.StunningAttack)
            {
                return DoAction_StunningAttack(AttackerAction, DefenderAction);
            }
            
            return new ActionEffectData();
        }

        private bool IsDead(CharacterBase Character)
        {
            return Character == null || Character.IsDead;
        }

        private ActionEffectData TryDecreaseStunDuration(CharacterBase Character)
        {
            if (Character.StunDuration > 0)
            {
                Character.StunDuration--;
                return new ActionEffectData("zzz...", "");
            }
            return new ActionEffectData();
        }

        private ActionEffectData DoAction_Attack(Action AttackerAction, Action DefenderAction)
        {
            DefenderAction.Owner.Health -= AttackerAction.Value;
            return new ActionEffectData("", (-AttackerAction.Value).ToString());
        }

        private ActionEffectData DoAction_SelfAttack(Action AttackerAction)
        {
            AttackerAction.Owner.Health -= AttackerAction.Value;
            return new ActionEffectData((-AttackerAction.Value).ToString(), "");
        }

        private ActionEffectData DoAction_RandomAttack(Action AttackerAction, Action DefenderAction)
        {
            bool success = UnityEngine.Random.value < 0.6f; // Fair random :)
            if (success)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            return new ActionEffectData("Miss", "");
        }

        private ActionEffectData DoAction_SelfRandomAttack(Action AttackerAction)
        {
            bool success = UnityEngine.Random.value < 0.6f; // Fair random :)
            if (success)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
            return new ActionEffectData("Miss", "");
        }

        private ActionEffectData DoAction_StunningAttack(Action AttackerAction, Action DefenderAction)
        {
            DefenderAction.Owner.StunDuration += AttackerAction.Duration;
            ActionEffectData data = DoAction_Attack(AttackerAction, DefenderAction);
            data.EnemyText += " Stun!";
            return data;
        }
    }
}