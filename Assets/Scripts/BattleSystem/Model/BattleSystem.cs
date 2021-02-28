using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class BattleSystem
    {
        public BattleSystem()
        {
            ActionBehaviour = new ActionExecutionBehaviour();
        }

        public System.Action OnTimerStarted;
        public System.Action OnTimerFinished;
        public System.Action<int> OnActionExecuted;

        private BattleTimelineTimer TimelineTimer;
        private ActionExecutionBehaviour ActionBehaviour;

        public List<CharacterBase> GetAlliedCharacters()
        {
            return GameInstance.Instance.GetAllies();
        }

        public List<CharacterBase> GetEnemyCharacters()
        {
            return GameInstance.Instance.GetEnemies();
        }

        public BattleTimelineTimer GetTimer()
        {
            return TimelineTimer;
        }

        public int GetTimelineLength()
        {
            return GetEnemyCharacters()[0].Skills.Aggregate(0, (total, next) => total += next.Length);
        }

        public void StartBattleTimer()
        {
            GameObject timerObject = new GameObject("Timer");
            TimelineTimer = timerObject.AddComponent<BattleTimelineTimer>();
            TimelineTimer.OnActionExecuted += (int Position) => OnActionExecuted?.Invoke(Position);
            TimelineTimer.OnElapsed += () => OnTimerFinished?.Invoke();
            TimelineTimer.Launch(5.0f, GetTimelineLength());

            OnTimerStarted?.Invoke();
        }

        public void ExecuteActions(Action AlliedAction, Action EnemyAction)
        {
            ActionBehaviour.Execute(AlliedAction, EnemyAction);
        }
    }
}