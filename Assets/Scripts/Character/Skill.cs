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
    }
}