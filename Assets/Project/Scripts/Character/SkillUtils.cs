using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Character
{
    public class SkillUtils
    {
        public static void RebuildRandomActionSkill(Skill SkillRef)
        {
            int rand = Random.Range(0, SkillRef.RandomActionsCounter);
            int randIndex = 0;

            foreach (Action action in SkillRef.Actions)
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
        }

        public static void RebuildAdrenalineSkill(List<Skill> SkillList, int Index, Skill SkillRef)
        {
            Dictionary<CharacterBase, int> adrenalineActions = new Dictionary<CharacterBase, int>();
            
            for (int i = 0; i < Index; ++i)
            {
                CharacterBase skillOwner = SkillList[i].Owner;
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
                return;
            }

            for (int i = 0; i < SkillRef.Actions.Count; ++i)
            {
                if (SkillRef.Actions[i].IsAdrenalineAction())
                {
                    adrenalineActions[owner]++;

                    if (adrenalineActions[owner] > owner.Adrenaline)
                    {
                        SkillRef.Actions[i].ActionType = CharacterActionType.AdrenalineCancelled;
                    }
                }
            }
        }

        public static void RebuildKeyOutSkill(List<Skill> SkillList, int Index, Skill SkillRef)
        {
            if (SkillList.Count <= Index + 1)
                return;

            Skill left = SkillRef;
            Skill right = SkillList[Index + 1];

            if (!AreSkillsKeysMatch(left, right))
                return;

            left.VirtualLength -= Mathf.Min(left.CountKeyOutActions(), right.CountKeyInActions());
        }

        public static bool AreSkillsKeysMatch(Skill Left, Skill Right)
        {
            if (!Left.IsKeyOutSkill() || !Right.IsKeyInSkill())
                return false;

            return Left.GetKeyOutForm() != Right.GetKeyInForm();
        }

        public static int CountAdrenalineActionsInSkill(Skill SkillRef)
        {
            return SkillRef.Actions.Aggregate(0, (total, action) => total += action.IsAdrenalineAction() ? 1 : 0);
        }

        public static bool IsOpeningSkill(Skill SkillRef)
        {
            if (SkillRef.Actions.Count == 0)
                return false;

            return SkillRef.Actions.First().ActionType == CharacterActionType.Open;
        }

        public static bool IsClosingSkill(Skill SkillRef)
        {
            if (SkillRef == null || SkillRef.Actions.Count == 0)
                return false;

            return SkillRef.Actions.Last().ActionType == CharacterActionType.Close;
        }

        public static List<Skill> GetOriginalSkillsFromCards(List<CardWrapper> Cards)
        {
            return Cards.Select(card => card.GetSkill()).ToList();
        }

        public static Skill GetOriginalSkill(Skill OldSkill)
        {
           return GameInstance.Instance.GetSkill(OldSkill.Owner.Name, OldSkill.Name);
        }
    }
}