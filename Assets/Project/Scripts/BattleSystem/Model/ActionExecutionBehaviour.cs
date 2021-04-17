﻿using System.Collections.Generic;
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
            List<ActionEffectData> actionsDataList = new List<ActionEffectData>();

            if (IsDead(AttackerAction.Owner))
                return actionsDataList;

            actionsDataList.Add(TryDecreaseStunDuration(AttackerAction.Owner));
            actionsDataList.Add(TryDecreaseBlockDuration(AttackerAction.Owner));
            actionsDataList.Add(TryDecreaseDodgeDuration(AttackerAction.Owner));
            actionsDataList.Add(TryDecreaseParryDuration(AttackerAction.Owner));

            if (AttackerAction.Owner.StunDuration > 0)
            {
                return actionsDataList;
            }

            if (AttackerAction.ActionType == CharacterActionType.Block)
            {
                actionsDataList.Add(DoAction_Block(AttackerAction));
            }
            if (AttackerAction.ActionType == CharacterActionType.LuckBlock)
            {
                actionsDataList.Add(DoAction_LuckBlock(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.Dodge)
            {
                actionsDataList.Add(DoAction_Dodge(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.LuckDodge)
            {
                actionsDataList.Add(DoAction_LuckDodge(AttackerAction));
            }
            else if (AttackerAction.ActionType == CharacterActionType.Parry)
            {
                actionsDataList.Add(DoAction_Parry(AttackerAction));
            }

            return actionsDataList;
        }

        private ActionEffectData Execute_Internal(Action AttackerAction, Action DefenderAction)
        {
            if (AttackerAction.Owner.StunDuration > 0)
                return null;

            if (AttackerAction.ActionType == CharacterActionType.ImperviousAttack)
            {
                return DoAction_ImperviousAttack(AttackerAction, DefenderAction);
            }
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
            if (AttackerAction.ActionType == CharacterActionType.AdrenalineAttack)
            {
                return DoAction_AdrenalineAttack(AttackerAction, DefenderAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.AdrenalineDodge)
            {
                return DoAction_AdrenalineDodge(AttackerAction);
            }
            if (AttackerAction.ActionType == CharacterActionType.AdrenalineBlock)
            {
                return DoAction_AdrenalineBlock(AttackerAction);
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
            if (DefenderAction.Owner.ParryDuration > 0)
            {
                return new ActionEffectData("", "Parry!");
            }

            int hitDamage = DefenderAction.Owner.Hit(AttackerAction.Value);
            ActionEffectData data = new ActionEffectData("", (-hitDamage).ToString());

            if (AttackerAction.Duration > 0)
            {
                DefenderAction.Owner.StunDuration = AttackerAction.Duration;
                data.EnemyText += " Stun!";
            }

            return data;
        }

        private ActionEffectData DoAction_Attack(Action AttackerAction, Action DefenderAction)
        {
            if (DefenderAction.Owner.DodgeDuration > 0)
            {
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
            return new ActionEffectData("Miss", "");
        }

        private ActionEffectData DoAction_SelfLuckAttack(Action AttackerAction)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return DoAction_SelfAttack(AttackerAction);
            }
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