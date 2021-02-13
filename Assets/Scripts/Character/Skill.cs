using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public class Skill
    {
        public Skill(List<Action> Actions)
        {
            this.Actions = Actions;
        }

        public List<Action> Actions;
        public int Length;
    }
}