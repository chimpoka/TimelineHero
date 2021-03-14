using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public class Skill
    {
        public Skill()
        {

        }

        public Skill(List<Action> Actions, int Length, CharacterBase Owner)
        {
            this.Actions = Actions;
            this.Length = Length;
            this.Owner = Owner;

            //Initialize();
        }

        public int RandomActionsCounter { get => randomActionsCounter; }

        public List<Action> Actions;
        public int Length;
        public CharacterBase Owner;

        private List<Action> AdditionalActions;
        private int randomActionsCounter;


        public void Initialize()
        {
            CountRandomActions();
            SplitCompositeActions();
        }

        public Action GetActionInPosition(int Position)
        {
            foreach (Action action in Actions)
            {
                if (action.Position == Position)
                {
                    return action;
                }
            }

            foreach (Action action in AdditionalActions)
            {
                if (action.Position == Position)
                {
                    return action;
                }
            }

            return new Action(CharacterActionType.Empty, Position, Owner);
        }

        public int CountActionsByType(CharacterActionType Type)
        {
            int actionsCount = 0;

            foreach(Action action in Actions)
            {
                if (action.ActionType == Type)
                {
                    actionsCount++;
                }
            }

            return actionsCount;
        }

        public bool HasActionsWithType(CharacterActionType Type)
        {
            foreach (Action action in Actions)
            {
                if (action.ActionType == Type)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAdrenalineSkill()
        {
            return (HasActionsWithType(CharacterActionType.AdrenalineAttack) ||
                    HasActionsWithType(CharacterActionType.AdrenalineDodge) ||
                    HasActionsWithType(CharacterActionType.AdrenalineBlock));
        }

        public bool IsRandomSkill()
        {
            return (HasActionsWithType(CharacterActionType.RandomAttack) ||
                    HasActionsWithType(CharacterActionType.SelfRandomAttack));
        }

        private void SplitCompositeActions()
        {
            AdditionalActions = AdditionalActions ?? new List<Action>();

            foreach (Action action in Actions)
            {
                for (int i = 1; i < action.Duration; ++i)
                {
                    if (action.IsAttackAction())
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.Stun, action.Position + i, Owner));
                    }
                    else if (action.IsBlockAction())
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.BlockContinuance, action.Position + i, Owner, action.Value));
                    }
                    else if (action.IsDodgeAction())
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.DodgeContinuance, action.Position + i, Owner));
                    }
                    else if (action.ActionType == CharacterActionType.Parry)
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.Parry, action.Position + i, Owner));
                    }
                }
            }
        }

        private void CountRandomActions()
        {
            foreach (Action action in Actions)
            {
                if (action.ActionType == CharacterActionType.RandomAttack ||
                    action.ActionType == CharacterActionType.SelfRandomAttack)
                {
                    randomActionsCounter++;
                }
            }
        }
    }
}