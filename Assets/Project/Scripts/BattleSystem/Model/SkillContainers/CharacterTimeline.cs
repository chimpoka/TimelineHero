using System.Collections.Generic;
using System.Linq;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class CharacterTimeline : SkillContainerBase
    {
        public int Length { get => Skills.Aggregate(0, (total, next) => total += next.Length); }
        public System.Action OnLengthChanged;

        private List<Action> ActualBattleActions = new List<Action>();

        override protected void OnContainerUpdated()
        {
            OnLengthChanged?.Invoke();
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