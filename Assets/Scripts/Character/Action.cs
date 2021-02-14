using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public enum CharacterActionType { Empty, Attack, Dodge, Stun, Block }
    [System.Serializable]
    public class Action
    {
        public Action(Action NewAction)
        {
            this.ActionType = NewAction.ActionType;
            this.Position = NewAction.Position;
        }

        public Action(CharacterActionType ActionType, int Position)
        {
            this.ActionType = ActionType;
            this.Position = Position;
        }

        public CharacterActionType ActionType;
        public int Position;
    }
}
