using System.Collections.Generic;
using System.Linq;

namespace TimelineHero.Character
{
    public class Skill
    {
        Skill() {}

        public Skill(Skill OldSkill)
        {
            this.Actions = new List<Action>(OldSkill.Actions);
            this.AdditionalActions = new List<Action>(OldSkill.AdditionalActions); // ?
            this.Length = OldSkill.Length;
            this.Owner = OldSkill.Owner;
            this.Name = OldSkill.Name;
        }

        public Skill(List<Action> Actions, int Length, CharacterBase Owner)
        {
            this.Actions = new List<Action>(Actions);
            this.Length = Length;
            this.Owner = Owner;

            //Initialize();
        }

        public List<Action> Actions;
        public int Length;
        public CharacterBase Owner;
        public string Name;

        private List<Action> AdditionalActions = new List<Action>();
        private int randomActionsCounter;


        public Skill Clone()
        {
            Skill newSkill = new Skill(this);
            newSkill.Actions = Actions.Select(action => action.Clone()).ToList();
            newSkill.AdditionalActions = AdditionalActions.Select(action => action.Clone()).ToList();

            return newSkill;
        }

        public void Initialize()
        {
            SplitCompositeActions();
        }

        public bool NeedsPreBattleRebuild()
        {
            return IsAdrenalineSkill() || IsKeyOutSkill();
        }

        public bool NeedsBattleRebuild()
        {
            return IsAdrenalineSkill() || IsKeyOutSkill() || IsRandomSkill();
        }

        public Action GetActionAtPosition(int Position)
        {
            Action main = Actions.Find((action) => action.Position == Position);

            if (main != null)
                return main;

            Action additional = AdditionalActions.Find((action) => action.Position == Position);

            if (additional != null)
                return additional;

            return new Action(CharacterActionType.Empty, Position, Owner);
        }

        public int CountActionsByType(CharacterActionType Type)
        {
            return Actions.Aggregate(0, (total, action) => action.ActionType == Type ? 1 : 0);
        }

        public bool HasActionsWithType(CharacterActionType Type)
        {
            return Actions.Find((action) => action.ActionType == Type) != null;
        }

        public ActionKeyForm GetKeyOutForm()
        {
            Action keyAction = Actions.Find((action) => action.IsKeyOutAction());
            return keyAction != null ? keyAction.KeyForm : ActionKeyForm.NoKey;
        }

        public ActionKeyForm GetKeyInForm()
        {
            Action keyAction = Actions.Find((action) => action.IsKeyInAction());
            return keyAction != null ? keyAction.KeyForm : ActionKeyForm.NoKey;
        }

        public int CountKeyInActions()
        {
            Action keyAction = Actions.Find((action) => action.IsKeyInAction());
            return keyAction != null ? keyAction.Duration : 0;
        }

        public int CountKeyOutActions()
        {
            Action keyAction = Actions.Find((action) => action.IsKeyOutAction());
            return keyAction != null ? keyAction.Duration : 0;
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

        public bool IsLuckSkill()
        {
            return (HasActionsWithType(CharacterActionType.LuckAttack) ||
                    HasActionsWithType(CharacterActionType.LuckBlock) ||
                    HasActionsWithType(CharacterActionType.LuckDodge) ||
                    HasActionsWithType(CharacterActionType.SelfLuckAttack));
        }

        public bool IsKeyInSkill()
        {
            return (HasActionsWithType(CharacterActionType.KeyIn1) ||
                    HasActionsWithType(CharacterActionType.KeyIn2) ||
                    HasActionsWithType(CharacterActionType.KeyIn3));
        }

        public bool IsKeyOutSkill()
        {
            return (HasActionsWithType(CharacterActionType.KeyOut1) ||
                    HasActionsWithType(CharacterActionType.KeyOut2) ||
                    HasActionsWithType(CharacterActionType.KeyOut3));
        }
        
        public int CountRandomActions()
        {
            return Actions.Aggregate(0, (total, action) => total += action.IsRandomAction() ? 1 : 0);
        }

        private void SplitCompositeActions()
        {
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
                    else if (action.IsKeyAction())
                    {
                        AdditionalActions.Add(new Action(action.ActionType, action.Position + i, Owner));
                    }
                }
            }
        }
    }
}