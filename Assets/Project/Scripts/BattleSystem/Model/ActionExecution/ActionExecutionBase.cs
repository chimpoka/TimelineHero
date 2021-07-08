using TimelineHero.Character;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    public enum ActionExecutionStage { PreExecute, Execute, PostExecute }

    public abstract class ActionExecutionBase
    {
        public ActionExecutionStage Stage;
        public CharacterActionType ActionType;

        public abstract ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null);
    }

    public class ImperviousAttackActionExecution : ActionExecutionBase
    {
        public ImperviousAttackActionExecution()
        {
            Stage = ActionExecutionStage.Execute;
            ActionType = CharacterActionType.ImperviousAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction)
        {
            if (DefenderAction.Owner.ParryDuration > 0 && AttackerAction.AttackType == DefenderAction.AttackType)
            {
                return new ActionEffectData("", "Parry! " + DefenderAction.AttackType.ToString(), true);
            }

            int hitDamage = DefenderAction.Owner.TakeDamage(AttackerAction.Value);
            ActionEffectData data = new ActionEffectData("", (-hitDamage).ToString(), true);
            data.Value = hitDamage;
            AttackerAction.SuccessfulAction = true;

            return data;
        }
    }

    public class AttackActionExecution : ImperviousAttackActionExecution
    {
        public AttackActionExecution()
        {
            ActionType = CharacterActionType.Attack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction)
        {
            if (DefenderAction.Owner.DodgeDuration > 0)
            {
                return new ActionEffectData("", "Dodged!", true);
            }

            return base.Execute(AttackerAction, DefenderAction);
        }
    }

    public class ExpendedHealthAttackActionExecution : AttackActionExecution
    {
        public ExpendedHealthAttackActionExecution()
        {
            ActionType = CharacterActionType.ExpendedHealthAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction)
        {
            AttackerAction.Value = AttackerAction.Owner.MaxHealth - AttackerAction.Owner.Health;
            return base.Execute(AttackerAction, DefenderAction);
        }
    }

    public class SelfAttackActionExecution : ActionExecutionBase
    {
        public SelfAttackActionExecution()
        {
            Stage = ActionExecutionStage.Execute;
            ActionType = CharacterActionType.SelfAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            int hitDamage = AttackerAction.Owner.TakeDamage(AttackerAction.Value);
            AttackerAction.SuccessfulAction = true;
            return new ActionEffectData((-hitDamage).ToString(), "", true);
        }
    }

    public class LuckAttackActionExecution : AttackActionExecution
    {
        public LuckAttackActionExecution()
        {
            ActionType = CharacterActionType.LuckAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return base.Execute(AttackerAction, DefenderAction);
            }

            return new ActionEffectData("Miss", "", true);
        }
    }

    public class SelfLuckAttackActionExecution : SelfAttackActionExecution
    {
        public SelfLuckAttackActionExecution()
        {
            ActionType = CharacterActionType.SelfLuckAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return base.Execute(AttackerAction);
            }

            return new ActionEffectData("Miss...", "", true);
        }
    }

    public class BlockActionExecution : ActionExecutionBase
    {
        public BlockActionExecution()
        {
            Stage = ActionExecutionStage.PreExecute;
            ActionType = CharacterActionType.Block;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.BlockDuration = AttackerAction.Duration;
                AttackerAction.Owner.Block = AttackerAction.Value;
                AttackerAction.SuccessfulAction = true;
            }

            return null;
        }
    }

    public class LuckBlockActionExecution : BlockActionExecution
    {
        public LuckBlockActionExecution()
        {
            ActionType = CharacterActionType.LuckBlock;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return base.Execute(AttackerAction);
            }

            return new ActionEffectData("Miss Block...", "", true);
        }
    }

    public class DodgeActionExecution : ActionExecutionBase
    {
        public DodgeActionExecution()
        {
            Stage = ActionExecutionStage.PreExecute;
            ActionType = CharacterActionType.Dodge;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.DodgeDuration = AttackerAction.Duration;
                AttackerAction.SuccessfulAction = true;
            }

            return null;
        }
    }

    public class LuckDodgeActionExecution : DodgeActionExecution
    {
        public LuckDodgeActionExecution()
        {
            ActionType = CharacterActionType.LuckDodge;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (!AttackerAction.DisabledInPlayState)
            {
                return base.Execute(AttackerAction);
            }

            return new ActionEffectData("Miss Dodge...", "", true);
        }
    }

    public class ParryActionExecution : ActionExecutionBase
    {
        public ParryActionExecution()
        {
            Stage = ActionExecutionStage.PreExecute;
            ActionType = CharacterActionType.Parry;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Duration > 0)
            {
                AttackerAction.Owner.ParryDuration = AttackerAction.Duration;
                AttackerAction.SuccessfulAction = true;
                return new ActionEffectData("", "");
            }

            return null;
        }
    }

    public class AdrenalineAttackActionExecution : AttackActionExecution
    {
        public AdrenalineAttackActionExecution()
        {
            ActionType = CharacterActionType.AdrenalineAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Owner.Adrenaline--;
            return base.Execute(AttackerAction, DefenderAction);
        }
    }

    public class FullAdrenalineAttackActionExecution : AttackActionExecution
    {
        public FullAdrenalineAttackActionExecution()
        {
            ActionType = CharacterActionType.FullAdrenalineAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Value = (int)(AttackerAction.Owner.Adrenaline * GameInstance.Get().FullAdrenalineAttackMultiplier);
            AttackerAction.Owner.Adrenaline = 0;
            return base.Execute(AttackerAction, DefenderAction);
        }
    }

    public class AdrenalineDodgeActionExecution : DodgeActionExecution
    {
        public AdrenalineDodgeActionExecution()
        {
            ActionType = CharacterActionType.AdrenalineDodge;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Owner.Adrenaline--;
            return base.Execute(AttackerAction);
        }
    }

    public class AdrenalineBlockActionExecution : BlockActionExecution
    {
        public AdrenalineBlockActionExecution()
        {
            ActionType = CharacterActionType.AdrenalineBlock;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            if (AttackerAction.Owner.Adrenaline <= 0)
            {
                UnityEngine.Debug.LogError("No Adrenaline!");
                return new ActionEffectData("Error: No Adrenaline!", "");
            }

            AttackerAction.Owner.Adrenaline--;
            return base.Execute(AttackerAction);
        }
    }

    public class DrawCardActionExecution : ActionExecutionBase
    {
        public DrawCardActionExecution()
        {
            Stage = ActionExecutionStage.Execute;
            ActionType = CharacterActionType.DrawCard;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            return new ActionEffectData("Draw " + AttackerAction.DrawCards + "\n(Deprecated)", "", true);
        }
    }

    public class DrawCardAttackActionExecution : AttackActionExecution
    {
        public DrawCardAttackActionExecution()
        {
            ActionType = CharacterActionType.DrawCardAttack;
        }

        public override ActionEffectData Execute(Action AttackerAction, Action DefenderAction = null)
        {
            ActionEffectData data = base.Execute(AttackerAction, DefenderAction);
            if (AttackerAction.SuccessfulAction && data.Value > 0)
            {
                return data + ActionExecutionPool.GetActionExecution(CharacterActionType.DrawCard)?.Execute(AttackerAction);
            }

            return data;
        }
    }
}