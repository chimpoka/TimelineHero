using System.Collections.Generic;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class Hand 
    {
        List<Skill> Skills = new List<Skill>();

        public void AddSkill(Skill NewSkill)
        {
            if (Skills.Contains(NewSkill))
                return;

            Skills.Add(NewSkill);
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
        }

        public void RemoveSkill(Skill NewSkill)
        {
            if (!Skills.Contains(NewSkill))
                return;

            Skills.Remove(NewSkill);
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
            return skills;
        }

        public List<Skill> GetSkills()
        {
            return Skills;
        }
    }
}