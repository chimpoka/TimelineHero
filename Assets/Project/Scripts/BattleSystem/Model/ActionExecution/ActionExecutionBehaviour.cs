using System.Collections.Generic;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class ActionData
    {
        public ActionData(List<ActionEffectData> EffectData)
        {
            this.EffectData = EffectData;

            IsSignificant = EffectData != null && EffectData.Find((effectData) => effectData != null && effectData.IsSignificant) != null;
        }

        public List<ActionEffectData> EffectData;
        public bool IsSignificant;
    }

    public class ActionEffectData
    {
        public ActionEffectData(string AlliedText, string EnemyText)
        {
            this.AlliedText = AlliedText;
            this.EnemyText = EnemyText;
        }

        public ActionEffectData(string AlliedText, string EnemyText, bool IsSignificant)
        {
            this.AlliedText = AlliedText;
            this.EnemyText = EnemyText;
            this.IsSignificant = IsSignificant;
        }

        public string AlliedText;
        public string EnemyText;
        public int Value;
        public bool IsSignificant;

        public static ActionEffectData operator +(ActionEffectData lhs, ActionEffectData rhs)
            => new ActionEffectData(lhs.AlliedText + rhs.AlliedText, lhs.EnemyText + rhs.EnemyText, lhs.IsSignificant || rhs.IsSignificant);

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
        public ActionData Execute(Action AlliedAction, Action EnemyAction)
        {
            List<ActionEffectData> effectsDataList = new List<ActionEffectData>();

            effectsDataList.AddRange(PreExecute_Internal(AlliedAction));
            effectsDataList.AddRange(SwapActionEffectData(PreExecute_Internal(EnemyAction)));

            if (IsDead(AlliedAction.Owner) || IsDead(EnemyAction.Owner))
            {
                return new ActionData(effectsDataList);
            }

            effectsDataList.Add(Execute_Internal(AlliedAction, EnemyAction));
            effectsDataList.Add(Execute_Internal(EnemyAction, AlliedAction)?.Swap());

            effectsDataList.Add(PostExecute_Internal(AlliedAction, EnemyAction));
            effectsDataList.Add(PostExecute_Internal(EnemyAction, AlliedAction)?.Swap());

            return new ActionData(effectsDataList);
        }

        private List<ActionEffectData> PreExecute_Internal(Action AttackerAction)
        {
            List<ActionEffectData> effectsDataList = new List<ActionEffectData>();

            if (IsDead(AttackerAction.Owner))
                return effectsDataList;

            effectsDataList.Add(TryDecreaseStunDuration(AttackerAction.Owner));
            effectsDataList.Add(TryDecreaseBlockDuration(AttackerAction.Owner));
            effectsDataList.Add(TryDecreaseDodgeDuration(AttackerAction.Owner));
            effectsDataList.Add(TryDecreaseParryDuration(AttackerAction.Owner));

            if (AttackerAction.Owner.StunDuration > 0)
                return effectsDataList;

            effectsDataList.Add(ActionExecutionPool.GetActionExecution(ActionExecutionStage.PreExecute, AttackerAction.ActionType)?.Execute(AttackerAction));

            return effectsDataList;
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (AttackerAction.Owner.StunDuration > 0)
                return null;

            return ActionExecutionPool.GetActionExecution(ActionExecutionStage.Execute, AttackerAction.ActionType)?.Execute(AttackerAction, DefenderAction);
        }

        private ActionEffectData PostExecute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (!AttackerAction.SuccessfulAction)
                return null;

            if (AttackerAction.IsAttackAction())
            {
                if (AttackerAction.Duration > 0)
                {
                    DefenderAction.Owner.StunDuration = AttackerAction.Duration;
                    return new ActionEffectData("", "\nStun!");
                }
            }

            return null;
        }

        private List<ActionEffectData> SwapActionEffectData(List<ActionEffectData> Data)
        {
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

        #region StatusDecrement
        private ActionEffectData TryDecreaseStunDuration(CharacterBase Character)
        {
            if (Character.StunDuration > 0)
            {
                Character.StunDuration--;

                if (Character.StunDuration > 0)
                {
                    return new ActionEffectData("zzz...", "");
                }
            }
            return null;
        }

        private ActionEffectData TryDecreaseBlockDuration(CharacterBase Character)
        {
            if (Character.BlockDuration > 0)
            {
                Character.BlockDuration--;

                if (Character.BlockDuration == 0)
                {
                    Character.Block = 0;
                }
            }
            return null;
        }

        private ActionEffectData TryDecreaseDodgeDuration(CharacterBase Character)
        {
            if (Character.DodgeDuration > 0)
            {
                Character.DodgeDuration--;
            }
            return null;
        }

        private ActionEffectData TryDecreaseParryDuration(CharacterBase Character)
        {
            if (Character.ParryDuration > 0)
            {
                Character.ParryDuration--;
            }
            return null;
        }
        #endregion StatusDecrement
    }
}