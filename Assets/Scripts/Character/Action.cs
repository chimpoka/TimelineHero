using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public enum CharacterActionType { Attack, Dodge, Stun, Block }
    [System.Serializable]
    public class Action
    {
        public CharacterActionType ActionType;
        public int Position;
    }
}
