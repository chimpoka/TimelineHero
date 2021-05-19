using System.Collections.Generic;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class SkillContainerBase
    {
        protected List<Skill> Skills = new List<Skill>();

        virtual public void SetSkills(List<Skill> NewSkills)
        {
            Skills = NewSkills;
            OnContainerUpdated();
        }

        virtual public void AddSkill(Skill NewSkill)
        {
            Skills.Add(NewSkill);
            OnContainerUpdated();
        }

        virtual public void AddSkills(List<Skill> NewSkills)
        {
            Skills.AddRange(NewSkills);
            OnContainerUpdated();
        }

        virtual public void RemoveSkill(Skill NewSkill)
        {
            Skills.Remove(NewSkill);
            OnContainerUpdated();
        }

        virtual public void RemoveSkills(List<Skill> NewSkills)
        {
            foreach (Skill skill in NewSkills)
            {
                Skills.Remove(skill);
            }
            OnContainerUpdated();
        }

        virtual public List<Skill> RemoveAllSkills()
        {
            List<Skill> skills = new List<Skill>(Skills);
            Skills.Clear();
            OnContainerUpdated();

            return skills;
        }

        virtual public List<Skill> GetSkills()
        {
            return Skills;
        }

        virtual protected void OnContainerUpdated()
        {
        }
    }
}