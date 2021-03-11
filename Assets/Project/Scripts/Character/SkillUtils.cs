using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimelineHero.Character
{
    public class SkillUtils
    {
        public static Skill GetRebuiltSkillWithRandomActions(Skill OldSkill)
        {
            int rand = Random.Range(0, OldSkill.RandomActionsCounter);
            int randIndex = 0;

            Skill newSkill = new Skill(OldSkill.Actions, OldSkill.Length, OldSkill.Owner);

            foreach (Action action in newSkill.Actions)
            {
                if (action.ActionType == CharacterActionType.RandomAttack)
                {
                    action.ActionType = rand == randIndex ? CharacterActionType.Attack : CharacterActionType.RandomAttackCancelled;
                    randIndex++;
                }
                else if (action.ActionType == CharacterActionType.SelfRandomAttack)
                {
                    action.ActionType = rand == randIndex ? CharacterActionType.SelfAttack : CharacterActionType.SelfRandomAttackCancelled;
                    randIndex++;
                }
            }

            newSkill.Initialize();

            return newSkill;
        }

        public static Skill GetRebuiltSkillWithAdrenaline(List<Skill> SkillList, int Index)
        {
            if (!SkillList[Index].IsAdrenalineSkill())
            {
                return SkillList[Index];
            }

            Dictionary<CharacterBase, int> adrenalineActions = new Dictionary<CharacterBase, int>();
            
            for (int i = 0; i < Index; ++i)
            {
                CharacterBase skillOwner = SkillList[Index].Owner;
                if (!adrenalineActions.ContainsKey(skillOwner))
                {
                    adrenalineActions[skillOwner] = 0;
                }
                adrenalineActions[skillOwner] += CountAdrenalineActionsInSkill(SkillList[i]);
            }

            CharacterBase owner = SkillList[Index].Owner;
            if (!adrenalineActions.ContainsKey(owner))
            {
                adrenalineActions[owner] = 0;
            }

            if (CountAdrenalineActionsInSkill(SkillList[Index]) + adrenalineActions[owner] <= owner.Adrenaline)
            {
                return SkillList[Index];
            }

            Skill newSkill = new Skill(SkillList[Index].Actions, SkillList[Index].Length, owner);

            for (int i = 0; i < newSkill.Actions.Count; ++i)
            {
                if (newSkill.Actions[i].IsAdrenalineAction())
                {
                    adrenalineActions[owner]++;

                    if (adrenalineActions[owner] > owner.Adrenaline)
                    {
                        newSkill.Actions[i].ActionType = CharacterActionType.AdrenalineCancelled;
                    }
                }
            }

            newSkill.Initialize();

            return newSkill;
        }

        public static int CountAdrenalineActionsInSkill(Skill SkillRef)
        {
            int result = 0;
            result += SkillRef.CountActionsByType(CharacterActionType.AdrenalineAttack);
            result += SkillRef.CountActionsByType(CharacterActionType.AdrenalineDodge);
            result += SkillRef.CountActionsByType(CharacterActionType.AdrenalineBlock);

            return result;
        }

        public static bool IsOpeningSkill(Skill SkillRef)
        {
            return SkillRef.Actions[0].ActionType == CharacterActionType.Open;
        }

        public static bool IsClosingSkill(Skill SkillRef)
        {
            return SkillRef?.Actions?.Last().ActionType == CharacterActionType.Close;
        }
    }
}