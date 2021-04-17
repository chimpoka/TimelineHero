﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Character
{
    public enum CharacterActionType
    {
        Empty, Attack, Dodge, Stun, Block, BlockContinuance, Parry, LuckAttack, LuckDodge, LuckBlock,
        AdrenalineAttack, AdrenalineDodge, AdrenalineBlock, RandomAttack, ImperviousAttack, SelfAttack,
        SelfLuckAttack, SelfRandomAttack, Open, Close, RandomAttackCancelled, SelfRandomAttackCancelled,
        DodgeContinuance, AdrenalineCancelled, KeyIn1, KeyIn2, KeyIn3, KeyOut1, KeyOut2, KeyOut3, 
        LuckCancelled
    }

    public enum CharacterAttackType
    {
        Hand, Weapon, Bullet
    }

    public enum ActionKeyForm { NoKey, Form1, Form2, Form3 }

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

            UpdateKeyForm();
        }

        public Action(CharacterActionType ActionType, int Position, CharacterBase Owner, int Value = 0)
        {
            this.ActionType = ActionType;
            this.Position = Position;
            this.Owner = Owner;
            this.Value = Value;
            this.Duration = 0;

            UpdateKeyForm();
        }

        public CharacterBase Owner;

        [HideLabel]
        [HorizontalGroup("Split", 130)]
        public CharacterActionType ActionType;

        [VerticalGroup("Split/Right")]
        [HideIf("@this.ActionType == CharacterActionType.Empty" +
             " || this.ActionType == CharacterActionType.Stun" +
             " || this.ActionType == CharacterActionType.BlockContinuance" +
             " || this.ActionType == CharacterActionType.RandomAttackCancelled" +
             " || this.ActionType == CharacterActionType.SelfRandomAttackCancelled" +
             " || this.ActionType == CharacterActionType.DodgeContinuance" +
             " || this.ActionType == CharacterActionType.AdrenalineCancelled")]
        public int Position;

        [VerticalGroup("Split/Right")]
        [ShowIf("@this.ActionType == CharacterActionType.Attack" +
             " || this.ActionType == CharacterActionType.Block" +
             " || this.ActionType == CharacterActionType.LuckAttack" +
             " || this.ActionType == CharacterActionType.LuckBlock" +
             " || this.ActionType == CharacterActionType.AdrenalineAttack" +
             " || this.ActionType == CharacterActionType.AdrenalineBlock" +
             " || this.ActionType == CharacterActionType.RandomAttack" +
             " || this.ActionType == CharacterActionType.ImperviousAttack" +
             " || this.ActionType == CharacterActionType.SelfAttack" +
             " || this.ActionType == CharacterActionType.SelfLuckAttack" +
             " || this.ActionType == CharacterActionType.SelfRandomAttack")]
        public int Value;

        [VerticalGroup("Split/Right")]
        [HideIf("@this.ActionType == CharacterActionType.Empty" +
             " || this.ActionType == CharacterActionType.Stun" +
             " || this.ActionType == CharacterActionType.BlockContinuance" +
             " || this.ActionType == CharacterActionType.Open" +
             " || this.ActionType == CharacterActionType.Close" +
             " || this.ActionType == CharacterActionType.RandomAttackCancelled" +
             " || this.ActionType == CharacterActionType.SelfRandomAttackCancelled" +
             " || this.ActionType == CharacterActionType.DodgeContinuance" +
             " || this.ActionType == CharacterActionType.AdrenalineCancelled")]
        public int Duration;

        [ShowIf("@this.ActionType == CharacterActionType.Attack" +
             " || this.ActionType == CharacterActionType.LuckAttack" +
             " || this.ActionType == CharacterActionType.AdrenalineAttack" +
             " || this.ActionType == CharacterActionType.RandomAttack" +
             " || this.ActionType == CharacterActionType.ImperviousAttack" +
             " || this.ActionType == CharacterActionType.SelfAttack" +
             " || this.ActionType == CharacterActionType.SelfLuckAttack" +
             " || this.ActionType == CharacterActionType.SelfRandomAttack")]
        [HideInInspector]
        public List<CharacterAttackType> AttackTypes;

        [HideInInspector]
        public ActionKeyForm KeyForm = ActionKeyForm.NoKey;
        [HideInInspector]
        public bool DisabledInPlayState = false;
        [HideInInspector]
        public bool SuccessfulAction = true;

        public Action Clone()
        {
            return new Action(this);
        }

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

        public bool IsRandomAction()
        {
            return (ActionType == CharacterActionType.RandomAttack ||
                    ActionType == CharacterActionType.SelfRandomAttack);
        }

        public bool IsLuckAction()
        {
            return (ActionType == CharacterActionType.LuckAttack ||
                    ActionType == CharacterActionType.LuckBlock ||
                    ActionType == CharacterActionType.LuckDodge||
                    ActionType == CharacterActionType.SelfLuckAttack);
        }

        public bool IsKeyInAction()
        {
            return (ActionType == CharacterActionType.KeyIn1 ||
                    ActionType == CharacterActionType.KeyIn2 ||
                    ActionType == CharacterActionType.KeyIn3);
        }

        public bool IsKeyOutAction()
        {
            return (ActionType == CharacterActionType.KeyOut1 ||
                    ActionType == CharacterActionType.KeyOut2 ||
                    ActionType == CharacterActionType.KeyOut3);
        }

        public bool IsKeyAction()
        {
            return IsKeyInAction() || IsKeyOutAction();
        }

        private void UpdateKeyForm()
        {
            if (ActionType == CharacterActionType.KeyIn1 || ActionType == CharacterActionType.KeyOut1)
            {
                KeyForm = ActionKeyForm.Form1;
            }
            else if (ActionType == CharacterActionType.KeyIn2 || ActionType == CharacterActionType.KeyOut2)
            {
                KeyForm = ActionKeyForm.Form2;
            }
            else if (ActionType == CharacterActionType.KeyIn3 || ActionType == CharacterActionType.KeyOut3)
            {
                KeyForm = ActionKeyForm.Form3;
            }
            else
            {
                KeyForm = ActionKeyForm.NoKey;
            }
        }
    }
}
