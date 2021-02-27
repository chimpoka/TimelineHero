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
            this.Owner = NewAction.Owner;
        }

        public Action(CharacterActionType ActionType, int Position, CharacterBase Owner)
        {
            this.ActionType = ActionType;
            this.Position = Position;
            this.Owner = Owner;
        }

        public CharacterActionType ActionType;
        public CharacterBase Owner;
        public int Position;
        public int Value;
    }
}
