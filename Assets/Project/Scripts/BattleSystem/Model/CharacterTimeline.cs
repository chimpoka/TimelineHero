using System.Collections.Generic;
using System.Linq;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class CharacterTimeline
    {
        public int Length { get => Skills.Aggregate(0, (total, next) => total += next.Length); }

        public System.Action OnLengthChanged;

        private List<Skill> Skills = new List<Skill>();
        private List<Action> ActualBattleActions = new List<Action>();

        public void AddSkill(Skill NewSkill)
        {
            if (Skills.Contains(NewSkill))
                return;

            Skills.Add(NewSkill);
            OnLengthChanged?.Invoke();
        }

        public void AddSkills(List<Skill> NewSkills)
        {
            foreach (Skill skill in NewSkills)
            {
                AddSkill(skill);
            }
        }

        public void SetSkills(List<Skill> NewSkills)
        {
            Skills = NewSkills;
            OnLengthChanged?.Invoke();
        }

        public bool RemoveSkill(Skill NewSkill)
        {
            if (!Skills.Contains(NewSkill))
                return false;

            Skills.Remove(NewSkill);
            OnLengthChanged?.Invoke();

            return true;
        }

        public void RemoveSkills(List<Skill> NewSkills)
        {
            foreach (Skill skill in NewSkills)
            {
                RemoveSkill(skill);
            }
        }

        public List<Skill> RemoveAllSkills()
        {
            List<Skill> skills = new List<Skill>(Skills);
            Skills.Clear();
            OnLengthChanged?.Invoke();

            return skills;
        }

        public Action GetActionAtPosition(int Position)
        {
            return Position >= ActualBattleActions.Count 
                ? new Action(CharacterActionType.Empty, Position, Skills.Last().Owner)
                : ActualBattleActions[Position];
        }

        public void CreateActualActions()
        {
            ActualBattleActions.Clear();

            foreach (Skill skill in Skills)
            {
                for (int i = 0; i < skill.Length; ++i)
                {
                    ActualBattleActions.Add(skill.GetActionAtPosition(i));
                }
            }
        }
    }
}