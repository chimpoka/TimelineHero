using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public enum CharacterActionType
    {
        Empty, Attack, Dodge, Stun, Block, BlockContinuance, Parry, LuckAttack, LuckDodge, LuckBlock,
        AdrenalineAttack, AdrenalineDodge, AdrenalineBlock, RandomAttack, ImperviousAttack, SelfAttack,
        SelfLuckAttack, SelfRandomAttack, Open, Close, RandomAttackCancelled, SelfRandomAttackCancelled,
        DodgeContinuance, AdrenalineCancelled
    }

    [System.Serializable]
    public class Action
    {
        public Action()
        {

        }

        public Action(Action NewAction)
        {
            this.ActionType = NewAction.ActionType;
            this.Position = NewAction.Position;
            this.Owner = NewAction.Owner;
            this.Value = NewAction.Value;
            this.Duration = NewAction.Duration;
        }

        public Action(CharacterActionType ActionType, int Position, CharacterBase Owner, int Value = 0)
        {
            this.ActionType = ActionType;
            this.Position = Position;
            this.Owner = Owner;
            this.Value = Value;
        }

        public CharacterActionType ActionType;
        public CharacterBase Owner;
        public int Position;
        public int Value;
        public int Duration;

        public bool IsAdrenalineAction()
        {
            return (ActionType == CharacterActionType.AdrenalineAttack ||
                    ActionType == CharacterActionType.AdrenalineDodge ||
                    ActionType == CharacterActionType.AdrenalineBlock);
        }

        public bool IsAttackAction()
        {
            return (ActionType == CharacterActionType.Attack ||
                    ActionType == CharacterActionType.LuckAttack ||
                    ActionType == CharacterActionType.RandomAttack ||
                    ActionType == CharacterActionType.SelfRandomAttack ||
                    ActionType == CharacterActionType.ImperviousAttack ||
                    ActionType == CharacterActionType.AdrenalineAttack);
        }

        public bool IsDodgeAction()
        {
            return (ActionType == CharacterActionType.Dodge ||
                    ActionType == CharacterActionType.AdrenalineDodge ||
                    ActionType == CharacterActionType.LuckDodge);
        }

        public bool IsBlockAction()
        {
            return (ActionType == CharacterActionType.Block ||
                    ActionType == CharacterActionType.LuckBlock ||
                    ActionType == CharacterActionType.AdrenalineBlock);
        }
    }
}
