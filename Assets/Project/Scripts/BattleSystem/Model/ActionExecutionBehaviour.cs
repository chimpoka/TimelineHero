using System.Collections.Generic;
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
            return AlliedText != null || EnemyText != null;
        }
    }

    public class ActionExecutionBehaviour
    {
        public List<ActionEffectData> Execute(Action AlliedAction, Action EnemyAction)
        {
            if (IsDead(AlliedAction.Owner) || IsDead(EnemyAction.Owner))
            {
                return null;
            }

            List<ActionEffectData> actionsDataList = new List<ActionEffectData>();

            ActionEffectData stunData;
            stunData = TryDecreaseStunDuration(AlliedAction.Owner);
            actionsDataList.Add(stunData.IsValid() ? stunData : Execute_Internal(AlliedAction, EnemyAction));
            stunData = TryDecreaseStunDuration(EnemyAction.Owner);
            actionsDataList.Add(stunData.IsValid() ? stunData.Swap() : Execute_Internal(EnemyAction, AlliedAction).Swap());

            return actionsDataList;
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (AttackerAction.ActionType == CharacterActionType.Attack)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.LuckAttack)
            {
                return DoAction_LuckAttack(AttackerAction, DefenderAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.SelfAttack)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.SelfLuckAttack)
            {
                return DoAction_SelfLuckAttack(AttackerAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.RandomAttack)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.SelfRandomAttack)
            {
                return DoAction_SelfAttack(AttackerAction);
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
            ActionEffectData data = new ActionEffectData("", (-AttackerAction.Value).ToString());

            if (AttackerAction.Duration > 0)
            {
                DefenderAction.Owner.StunDuration += AttackerAction.Duration;
                data.EnemyText += " Stun!";
            }

            return data;
        }

        private ActionEffectData DoAction_SelfAttack(Action AttackerAction)
        {
            AttackerAction.Owner.Health -= AttackerAction.Value;
            return new ActionEffectData((-AttackerAction.Value).ToString(), "");
        }

        private ActionEffectData DoAction_LuckAttack(Action AttackerAction, Action DefenderAction)
        {
            bool success = UnityEngine.Random.value < 0.6f; // Fair random :)
            if (success)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            return new ActionEffectData("Miss", "");
        }

        private ActionEffectData DoAction_SelfLuckAttack(Action AttackerAction)
        {
            bool success = UnityEngine.Random.value < 0.6f; // Fair random :)
            if (success)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
            return new ActionEffectData("Miss", "");
        }
    }
}