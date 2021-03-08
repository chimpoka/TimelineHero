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