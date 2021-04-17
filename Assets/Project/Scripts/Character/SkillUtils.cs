using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Character
{
    public class SkillUtils
    {
        public static void RebuildRandomActionSkill(Skill InOutSkillRef)
        {
            int rand = Random.Range(0, InOutSkillRef.CountRandomActions());
            int randIndex = 0;

            foreach (Action action in InOutSkillRef.Actions)
            {
                if (action.ActionType == CharacterActionType.RandomAttack)
                {
                    action.ActionType = rand == randIndex ? CharacterActionType.Attack : CharacterActionType.RandomAttackCancelled;
                    action.DisabledInPlayState = true;
                    randIndex++;
                }
                else if (action.ActionType == CharacterActionType.SelfRandomAttack)
                {
                    action.ActionType = rand == randIndex ? CharacterActionType.SelfAttack : CharacterActionType.SelfRandomAttackCancelled;
                    action.DisabledInPlayState = true;
                    randIndex++;
                }
            }
        }

        public static void RebuildAdrenalineSkill(List<Skill> SkillList, int Index, Skill InOutSkillRef)
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

            for (int i = 0; i < InOutSkillRef.Actions.Count; ++i)
            {
                if (InOutSkillRef.Actions[i].IsAdrenalineAction())
                {
                    adrenalineActions[owner]++;

                    if (adrenalineActions[owner] > owner.Adrenaline)
                    {
                        InOutSkillRef.Actions[i].ActionType = CharacterActionType.AdrenalineCancelled;
                        InOutSkillRef.Actions[i].DisabledInPlayState = true;
                    }
                }
            }
        }

        public static void RebuildKeyOutSkill(List<Skill> SkillList, int Index, Skill InOutSkillRef)
        {
            if (SkillList.Count <= Index + 1)
                return;

            Skill left = InOutSkillRef;
            Skill right = SkillList[Index + 1];

            if (!AreSkillsKeysMatch(left, right))
                return;

            left.BoardLength -= Mathf.Min(left.CountKeyOutActions(), right.CountKeyInActions());
        }

        public static void RebuildLuckSkill(Skill InOutSkillRef)
        {
            foreach(Action action in InOutSkillRef.Actions)
            {
                if (action.IsLuckAction())
                {
                    RebuildLuckAction(action);
                }
            }
        }

        public static void RebuildLuckAction(Action InOutActionRef)
        {
            bool success = UnityEngine.Random.value < 0.6f; // Fair random :)
            if (success)
                return;

            InOutActionRef.ActionType = CharacterActionType.LuckCancelled;
            InOutActionRef.DisabledInPlayState = true;
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

        public static void CopyCardStats(Card FromCard, Card ToCard)
        {
            for (int i = 0; i < FromCard.Steps.Count; ++i)
            {
                ToCard.Steps[i].DisabledInPlayState = FromCard.Steps[i].DisabledInPlayState;
            }
        }
    }
}