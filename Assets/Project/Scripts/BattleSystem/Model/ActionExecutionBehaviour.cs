using System.Collections.Generic;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class ActionEffectData
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
        public List<ActionEffectData> Execute(Action AlliedAction, Action EnemyAction)
        {
            List<ActionEffectData> actionsDataList = new List<ActionEffectData>();

            actionsDataList.AddRange(PreExecute_Internal(AlliedAction));
            actionsDataList.AddRange(SwapActionEffectData(PreExecute_Internal(EnemyAction)));

            if (IsDead(AlliedAction.Owner) || IsDead(EnemyAction.Owner))
            {
                return actionsDataList;
            }

            actionsDataList.Add(Execute_Internal(AlliedAction, EnemyAction));
            actionsDataList.Add(Execute_Internal(EnemyAction, AlliedAction)?.Swap());

            return actionsDataList;
        }

        private List<ActionEffectData> PreExecute_Internal(Action AttackerAction)
        {
            if (IsDead(AttackerAction.Owner))
                return null;
            
            List<ActionEffectData> actionsDataList = new List<ActionEffectData>();

            actionsDataList.Add(TryDecreaseStunDuration(AttackerAction.Owner));

            if (AttackerAction.Owner.StunDuration > 0)
            {
                return actionsDataList;
            }

            actionsDataList.Add(TryDecreaseBlockDuration(AttackerAction.Owner));

            if (AttackerAction.ActionType == CharacterActionType.Block)
            {
                actionsDataList.Add(DoAction_Block(AttackerAction));
            }
            
            return actionsDataList;
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (AttackerAction.Owner.StunDuration > 0)
                return null;

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

            return null;
        }

        private List<ActionEffectData> SwapActionEffectData(List<ActionEffectData> Data)
        {
            if (Data == null)
                return null;

            for (int i = 0; i < Data.Count; ++i)
            {
                Data[i]?.Swap();
            }

            return Data;
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
            return null;
        }

        private ActionEffectData TryDecreaseBlockDuration(CharacterBase Character)
        {
            if (Character.BlockDuration > 0)
            {
                Character.BlockDuration--;
            }
            else
            {
                Character.Block = 0;
            }
            return null;
        }

        private ActionEffectData DoAction_Attack(Action AttackerAction, Action DefenderAction)
        {
            int hitDamage = DefenderAction.Owner.Hit(AttackerAction.Value);
            ActionEffectData data = new ActionEffectData("", (-hitDamage).ToString());

            if (AttackerAction.Duration > 0)
            {
                DefenderAction.Owner.StunDuration += AttackerAction.Duration - 1;
                data.EnemyText += " Stun!";
            }

            return data;
        }

        private ActionEffectData DoAction_SelfAttack(Action AttackerAction)
        {
            int hitDamage = AttackerAction.Owner.Hit(AttackerAction.Value);
            return new ActionEffectData((-hitDamage).ToString(), "");
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

        private ActionEffectData DoAction_Block(Action AttackerAction)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.BlockDuration += AttackerAction.Duration - 1;
                AttackerAction.Owner.Block = AttackerAction.Value;
                return new ActionEffectData("Block!", "");
            }
            return null;
        }
    }
}