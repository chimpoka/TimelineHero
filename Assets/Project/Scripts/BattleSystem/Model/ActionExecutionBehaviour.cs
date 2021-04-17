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
            List<ActionEffectData> effectsDataList = new List<ActionEffectData>();

            effectsDataList.AddRange(PreExecute_Internal(AlliedAction));
            effectsDataList.AddRange(SwapActionEffectData(PreExecute_Internal(EnemyAction)));

            if (IsDead(AlliedAction.Owner) || IsDead(EnemyAction.Owner))
            {
                return effectsDataList;
            }

            effectsDataList.Add(Execute_Internal(AlliedAction, EnemyAction));
            effectsDataList.Add(Execute_Internal(EnemyAction, AlliedAction)?.Swap());

            effectsDataList.Add(PostExecute_Internal(AlliedAction, EnemyAction));
            effectsDataList.Add(PostExecute_Internal(EnemyAction, AlliedAction)?.Swap());

            return effectsDataList;
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

            if (AttackerAction.ActionType == CharacterActionType.Block)
            {
                effectsDataList.Add(DoAction_Block(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.LuckBlock)
            {
                effectsDataList.Add(DoAction_LuckBlock(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.AdrenalineBlock)
            {
                effectsDataList.Add(DoAction_AdrenalineBlock(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.Dodge)
            {
                effectsDataList.Add(DoAction_Dodge(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.LuckDodge)
            {
                effectsDataList.Add(DoAction_LuckDodge(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.AdrenalineDodge)
            {
                effectsDataList.Add(DoAction_AdrenalineDodge(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.Parry)
            {
                effectsDataList.Add(DoAction_Parry(AttackerAction));
            }

            return effectsDataList;
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (AttackerAction.Owner.StunDuration > 0)
                return null;

            if (AttackerAction.ActionType == CharacterActionType.ImperviousAttack)
            {
                return DoAction_ImperviousAttack(AttackerAction, DefenderAction);
            }
            else if(AttackerAction.ActionType == CharacterActionType.Attack)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            else if (AttackerAction.ActionType == CharacterActionType.LuckAttack)
            {
                return DoAction_LuckAttack(AttackerAction, DefenderAction);
            }
            else if (AttackerAction.ActionType == CharacterActionType.SelfAttack)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
            else if (AttackerAction.ActionType == CharacterActionType.SelfLuckAttack)
            {
                return DoAction_SelfLuckAttack(AttackerAction);
            }
            else if (AttackerAction.ActionType == CharacterActionType.RandomAttack)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }
            else if (AttackerAction.ActionType == CharacterActionType.SelfRandomAttack)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
            else if (AttackerAction.ActionType == CharacterActionType.AdrenalineAttack)
            {
                return DoAction_AdrenalineAttack(AttackerAction, DefenderAction);
            }

            return null;
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
                    return new ActionEffectData("", "        Stun!");
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

        private ActionEffectData DoAction_ImperviousAttack(Action AttackerAction, Action DefenderAction)
        {
            if (DefenderAction.Owner.ParryDuration > 0 && AttackerAction.AttackType == DefenderAction.AttackType)
            {
                AttackerAction.SuccessfulAction = false;
                return new ActionEffectData("", "Parry! " + DefenderAction.AttackType.ToString());
            }

            int hitDamage = DefenderAction.Owner.Hit(AttackerAction.Value);
            ActionEffectData data = new ActionEffectData("", (-hitDamage).ToString());

            return data;
        }

        private ActionEffectData DoAction_Attack(Action AttackerAction, Action DefenderAction)
        {
            if (DefenderAction.Owner.DodgeDuration > 0)
            {
                AttackerAction.SuccessfulAction = false;
                return new ActionEffectData("", "Dodged!");
            }

            return DoAction_ImperviousAttack(AttackerAction, DefenderAction);
        }

        private ActionEffectData DoAction_SelfAttack(Action AttackerAction)
        {
            int hitDamage = AttackerAction.Owner.Hit(AttackerAction.Value);
            return new ActionEffectData((-hitDamage).ToString(), "");
        }

        private ActionEffectData DoAction_LuckAttack(Action AttackerAction, Action DefenderAction)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return DoAction_Attack(AttackerAction, DefenderAction);
            }

            AttackerAction.SuccessfulAction = false;
            return new ActionEffectData("Miss", "");
        }

        private ActionEffectData DoAction_SelfLuckAttack(Action AttackerAction)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return DoAction_SelfAttack(AttackerAction);
            }

            AttackerAction.SuccessfulAction = false;
            return new ActionEffectData("Miss...", "");
        }

        private ActionEffectData DoAction_Block(Action AttackerAction)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.BlockDuration = AttackerAction.Duration;
                AttackerAction.Owner.Block = AttackerAction.Value;
                return new ActionEffectData("Block opened!", "");
            }
            return null;
        }

        private ActionEffectData DoAction_LuckBlock(Action AttackerAction)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return DoAction_Block(AttackerAction);
            }
            return new ActionEffectData("Miss Block...", "");
        }

        private ActionEffectData DoAction_Dodge(Action AttackerAction)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.DodgeDuration = AttackerAction.Duration;
                return new ActionEffectData("Dodge started!", "");
            }
            return null;
        }

        private ActionEffectData DoAction_LuckDodge(Action AttackerAction)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return DoAction_Dodge(AttackerAction);
            }
            return new ActionEffectData("Miss Dodge...", "");
        }

        private ActionEffectData DoAction_Parry(Action AttackerAction)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.ParryDuration = AttackerAction.Duration;
            }
            return null;
        }

        private ActionEffectData DoAction_AdrenalineAttack(Action AttackerAction, Action DefenderAction)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Owner.Adrenaline--;
            return DoAction_Attack(AttackerAction, DefenderAction);
        }

        private ActionEffectData DoAction_AdrenalineDodge(Action AttackerAction)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Owner.Adrenaline--;
            return DoAction_Dodge(AttackerAction);
        }

        private ActionEffectData DoAction_AdrenalineBlock(Action AttackerAction)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Owner.Adrenaline--;
            return DoAction_Block(AttackerAction);
        }
    }
}