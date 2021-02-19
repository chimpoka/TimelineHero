using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public enum CharacterActionType
    {
        Empty, Attack, Dodge, Stun, Block, BlockContinuance, Parry, LuckAttack, LuckDodge, LuckBlock,
        AdrenalinAttack, AdrenalinDodge, AdrenalinBlock, RandomAttack, ImperviousAttack, SelfAttack,
        SelfLuckAttack, SelfRandomAttack, Open, Close
    }

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
