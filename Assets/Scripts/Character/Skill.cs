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

            SplitCompositeActions();
        }

        public List<Action> Actions;
        public int Length;
        public CharacterBase Owner;

        public Action GetActionInPosition(int Position)
        {
            foreach(Action action in Actions)
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
            List<Action> actionsToAdd = new List<Action>();

            foreach (Action action in Actions)
            {
                for (int i = 1; i < action.Duration; ++i)
                {
                    if (action.ActionType == CharacterActionType.StunningAttack)
                    {
                        actionsToAdd.Add(new Action(CharacterActionType.Stun, action.Position + i, Owner));
                    }
                }
            }

            Actions.AddRange(actionsToAdd);
        }
    }
}