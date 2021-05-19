using System.Collections.Generic;
using TimelineHero.Character;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class DrawDeck : SkillContainerBase
    {
        public System.Action<int> OnDeckSizeChanged;

        override public void AddSkills(List<Skill> NewSkills)
        {
            base.AddSkills(NewSkills);
            
            if (GameInstance.Instance.ShuffleDrawDeck)
            {
                Shuffle();
            }
        }

        override protected void OnContainerUpdated()
        {
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }

        public List<Skill> Draw(int Count)
        {
            if (Skills.Count == 0)
                return new List<Skill>();

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

            OnDeckSizeChanged?.Invoke(Skills.Count);

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