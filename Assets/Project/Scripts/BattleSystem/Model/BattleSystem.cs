using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;
using TimelineHero.Character;
using TimelineHero.Config;

namespace TimelineHero.Battle
{
    public enum BattleResult { Lose, Win }

    public class BattleSystem
    {
        public BattleSystem()
        {
            ActionBehaviour = new ActionExecutionBehaviour();

            foreach (CharacterBase enemy in GetEnemyCharacters())
            {
                enemy.OnDied += OnEnemyDied;
            }

            foreach(CharacterBase ally in GetAlliedCharacters())
            {
                ally.OnDied += OnAllyDied;
            }
        }

        public System.Action OnTimerStarted;
        public System.Action OnTimerFinished;
        public System.Action<int> OnTimerIntegerValue;
        public System.Action<float> OnTimerInterpValue;

        public System.Action<List<ActionEffectData>> OnActionExecuted;
        public System.Action<BattleResult> OnBattleFinished;

        public int CurrentEnemyIndex = -1;

        private BattleTimelineTimer TimelineTimer;
        private ActionExecutionBehaviour ActionBehaviour;
        private List<CharacterBase> AlliedCharacters = new List<CharacterBase>();
        private List<CharacterBase> EnemyCharacters = new List<CharacterBase>();

        public List<CharacterBase> GetAlliedCharacters()
        {
            if (AlliedCharacters.Count == 0)
            {
                AlliedCharacters = GameInstance.Instance.GetAllies();
            }
            return AlliedCharacters;
        }

        public List<CharacterBase> GetEnemyCharacters()
        {
            if (EnemyCharacters.Count == 0)
            {
                EnemyCharacters = GameInstance.Instance.GetEnemies();
            }
            return EnemyCharacters;
        }

        public CharacterBase GetCurrentEnemy()
        {
            return GetEnemyCharacters()[CurrentEnemyIndex];
        }

        public int GetTimelineLength()
        {
            return GetEnemyCharacters()[CurrentEnemyIndex].Skills.Aggregate(0, (total, next) => total += next.Length);
        }

        public void StartBattleTimer()
        {
            GameObject timerObject = new GameObject("Timer");
            TimelineTimer = timerObject.AddComponent<BattleTimelineTimer>();
            TimelineTimer.OnIntegerValue += (int Position) => OnTimerIntegerValue?.Invoke(Position);
            TimelineTimer.OnUpdateInterp += (float Position) => OnTimerInterpValue?.Invoke(Position);
            TimelineTimer.OnElapsed += () => OnTimerFinished?.Invoke();
            TimelineTimer.OnStopped += () => OnTimerFinished?.Invoke();
            TimelineTimer.Launch(2.0f, GetTimelineLength());

            OnTimerStarted?.Invoke();
        }

        public void StopBattleTimer()
        {
            TimelineTimer.Stop();
        }
        public void ExecuteActions(Action AlliedAction, Action EnemyAction)
        {
            List<ActionEffectData> data = ActionBehaviour.Execute(AlliedAction, EnemyAction);
            OnActionExecuted?.Invoke(data);
        }

        private void OnEnemyDied(CharacterBase Enemy)
        {
            OnBattleFinished?.Invoke(BattleResult.Win);
        }

        private void OnAllyDied(CharacterBase Ally)
        {
            foreach(CharacterBase character in GetAlliedCharacters())
            {
                if (!character.IsDead)
                {
                    return;
                }
            }

            OnBattleFinished?.Invoke(BattleResult.Lose);
        }
    }
}