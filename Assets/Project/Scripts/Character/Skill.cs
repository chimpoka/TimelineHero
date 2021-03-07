using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public class Skill
    {
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

        private void SplitCompositeActions()
        {
            AdditionalActions = AdditionalActions ?? new List<Action>();

            foreach (Action action in Actions)
            {
                for (int i = 1; i < action.Duration; ++i)
                {
                    if (action.ActionType == CharacterActionType.Attack ||
                        action.ActionType == CharacterActionType.LuckAttack ||
                        action.ActionType == CharacterActionType.RandomAttack ||
                        action.ActionType == CharacterActionType.SelfRandomAttack)
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.Stun, action.Position + i, Owner));
                    }
                    else if (action.ActionType == CharacterActionType.Block)
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.BlockContinuance, action.Position + i, Owner, action.Value));
                    }
                    else if (action.ActionType == CharacterActionType.Dodge ||
                             action.ActionType == CharacterActionType.LuckDodge)
                    {
                        AdditionalActions.Add(new Action(CharacterActionType.Dodge, action.Position + i, Owner));
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