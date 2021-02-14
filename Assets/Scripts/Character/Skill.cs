using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public class Skill
    {
        public Skill(List<Action> Actions, int Length)
        {
            this.Actions = Actions;
            this.Length = Length;
        }

        public List<Action> Actions;
        public int Length;

        public Action GetActionInPosition(int Position)
        {
            foreach(Action action in Actions)
            {
                if (action.Position == Position)
                {
                    return action;
                }
            }

            return null;
        }
    }
}