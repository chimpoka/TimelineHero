using System.Collections.Generic;
using TimelineHero.Character;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class DrawDeck
    {
        public List<Skill> Skills = new List<Skill>();

        public System.Action<int> OnDeckSizeChanged;

        public void Add(List<Skill> SkillsToAdd)
        {
            Skills.AddRange(SkillsToAdd);

            if (GameInstance.Instance.ShuffleDrawDeck)
            {
                Shuffle();
            }

            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public List<Skill> Draw(int Count)
        {
            if (Skills.Count == 0)
                return null;

            if (Skills.Count > 0 && Skills.Count < Count)
            {
                return Remove(Skills.Count);
            }
            else if (Skills.Count >= Count)
            {
                return Remove(Count);
            }

            return null;
        }

        public List<Skill> Remove(int Count)
        {
            List<Skill> skills = Skills.GetRange(0, Count);
            Skills.RemoveRange(0, Count);
            return skills;
        }

        private void Shuffle()
        {
            int n = Skills.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                Skill value = Skills[k];
                Skills[k] = Skills[n];
                Skills[n] = value;
            }
        }
    }
}