using System.Collections.Generic;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class DiscardDeck
    {
        public DiscardDeck()
        {
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public List<Skill> Skills = new List<Skill>();

        public System.Action<int> OnDeckSizeChanged;

        public void Add(Skill SkillToAdd)
        {
            Skills.Add(SkillToAdd);
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public void Add(List<Skill> SkillsToAdd)
        {
            Skills.AddRange(SkillsToAdd);
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public List<Skill> RemoveAllFromDeck()
        {
            List<Skill> result = new List<Skill>(Skills);
            Skills.Clear();
            OnDeckSizeChanged?.Invoke(Skills.Count);

            return result;
        }
    }
}