using System.Collections.Generic;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class DiscardDeck : SkillContainerBase
    {
        public System.Action<int> OnDeckSizeChanged;

        override protected void OnContainerUpdated()
        {
            OnDeckSizeChanged?.Invoke(Skills.Count);
        }
    }
}