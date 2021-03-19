using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Character
{
    public class SkillUtils
    {
        public static Skill GetRebuiltSkillWithRandomActions(Skill OldSkill)
        {
            int rand = Random.Range(0, OldSkill.RandomActionsCounter);
            int randIndex = 0;

            Skill newSkill = OldSkill;

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

            Skill newSkill = SkillList[Index];

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
            return SkillRef.Actions.Aggregate(0, (total, action) => total += action.IsAdrenalineAction() ? 1 : 0);
        }

        public static bool IsOpeningSkill(Skill SkillRef)
        {
            return SkillRef.Actions[0].ActionType == CharacterActionType.Open;
        }

        public static bool IsClosingSkill(Skill SkillRef)
        {
            return SkillRef?.Actions?.Last().ActionType == CharacterActionType.Close;
        }

        public static List<Skill> GetOriginalSkillsFromCards(List<Card> Cards)
        {
            return Cards.Select(card => GetOriginalSkill(card.GetSkill())).ToList();
        }

        public static Skill GetOriginalSkill(Skill OldSkill)
        {
           return GameInstance.Instance.GetSkill(OldSkill.Owner.Name, OldSkill.Name);
        }
    }
}