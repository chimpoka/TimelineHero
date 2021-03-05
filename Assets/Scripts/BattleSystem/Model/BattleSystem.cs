﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public enum BattleResult { Lost, Won }

    public class BattleSystem
    {
        public BattleSystem()
        {
            ActionBehaviour = new ActionExecutionBehaviour();

            GetEnemyCharacters()[0].OnDied += OnEnemyDied;

            foreach(CharacterBase ally in GetAlliedCharacters())
            {
                ally.OnDied += OnAllyDied;
            }
        }

        public System.Action OnTimerStarted;
        public System.Action OnTimerFinished;
        public System.Action<int> OnTimerIntegerValue;
        public System.Action<ActionEffectData[]> OnActionExecuted;
        public System.Action<BattleResult> OnBattleFinished;

        private BattleTimelineTimer TimelineTimer;
        private ActionExecutionBehaviour ActionBehaviour;
        private List<CharacterBase> AlliedCharacters;
        private List<CharacterBase> EnemyCharacters;

        public List<CharacterBase> GetAlliedCharacters()
        {
            if (AlliedCharacters == null || AlliedCharacters.Count == 0)
            {
                AlliedCharacters = GameInstance.Instance.GetAllies();
            }
            return AlliedCharacters;
        }

        public List<CharacterBase> GetEnemyCharacters()
        {
            if (EnemyCharacters == null || EnemyCharacters.Count == 0)
            {
                EnemyCharacters = GameInstance.Instance.GetEnemies();
            }
            return EnemyCharacters;
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
            TimelineTimer.OnIntegerValue += (int Position) => OnTimerIntegerValue?.Invoke(Position);
            TimelineTimer.OnElapsed += () => OnTimerFinished?.Invoke();
            TimelineTimer.OnStopped += () => OnTimerFinished?.Invoke();
            TimelineTimer.Launch(5.0f, GetTimelineLength());

            OnTimerStarted?.Invoke();
        }

        public void StopBattleTimer()
        {
            TimelineTimer.Stop();
        }

        public void ExecuteActions(Action AlliedAction, Action EnemyAction)
        {
            ActionEffectData[] data = ActionBehaviour.Execute(AlliedAction, EnemyAction);
            OnActionExecuted?.Invoke(data);
        }

        private void OnEnemyDied(CharacterBase Enemy)
        {
            OnBattleFinished?.Invoke(BattleResult.Won);
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

            OnBattleFinished?.Invoke(BattleResult.Lost);
        }
    }
}