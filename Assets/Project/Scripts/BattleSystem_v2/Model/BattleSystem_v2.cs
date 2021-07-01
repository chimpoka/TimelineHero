using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.Battle_v2
{
    public class BattleSystem_v2 : Core.Subsystem<BattleSystem_v2>
    {
        public System.Action OnTimerStarted;
        public System.Action OnTimerFinished;
        public System.Action<int> OnTimerIntegerValue;
        public System.Action<float> OnTimerInterpValue;

        public System.Action<ActionData> OnActionExecuted;
        public System.Action<BattleResult> OnBattleFinished;

        public System.Action OnDiscardAllCardsFromTimeline;
        public System.Action OnEnemyChanged;

        public System.Action OnInitialBattleState;
        public System.Action OnMainBattleState;
        public System.Action OnPlayBattleState;

        public System.Action<Equipment, EquipmentSlot> OnBattleEquipmentCreationCommited;

        public int CurrentEnemyIndex = 0;
        public int CurrentEnemySkillSetIndex = -1;

        public Board BattleBoard = new Board();

        private BattleTimelineTimer TimelineTimer;
        private ActionExecutionBehaviour ActionBehaviour = new ActionExecutionBehaviour();


        protected override void OnInitialize() 
        {
            CharacterPool.GetEnemyCharacters().ForEach(x => x.OnDied += OnEnemyDied);
            CharacterPool.GetAlliedCharacters().ForEach(x => x.OnDied += OnAllyDied);
            OnTimerIntegerValue += ExecuteActionsAtPosition;
        }

        public void SetInitialBattleState() 
        {
            DiscardAllCardsFromTimeline();
            CreateNextEnemySkillSet();
            OnInitialBattleState?.Invoke();
        }

        public void SetMainBattleState() 
        {
            OnMainBattleState?.Invoke();
        }

        public void SetPlayBattleState()
        {
            StartBattleTimer();

            OnPlayBattleState?.Invoke();
        }

        public void DiscardAllCardsFromTimeline() { }

        public void CreateNextEnemySkillSet()
        {
            CharacterBase currentEnemy = GetCurrentEnemy();
            CurrentEnemySkillSetIndex = (CurrentEnemySkillSetIndex + 1) % currentEnemy.SkillSets.Count;

            OnEnemyChanged?.Invoke();
        }

        public void CreatePreviousEnemySkillSet()
        {
            CharacterBase currentEnemy = GetCurrentEnemy();
            int count = currentEnemy.SkillSets.Count;
            CurrentEnemySkillSetIndex = (CurrentEnemySkillSetIndex + count - 1) % count;

            OnEnemyChanged?.Invoke();
        }

        public void CreateNextEnemy()
        {
            List<CharacterBase> enemies = CharacterPool.GetEnemyCharacters();
            CurrentEnemySkillSetIndex = 0;
            CurrentEnemyIndex = (CurrentEnemyIndex + 1) % enemies.Count;

            OnEnemyChanged?.Invoke();
        }

        public void CreatePreviousEnemy()
        {
            List<CharacterBase> enemies = CharacterPool.GetEnemyCharacters();
            CurrentEnemySkillSetIndex = 0;
            CurrentEnemyIndex = (CurrentEnemyIndex + enemies.Count - 1) % enemies.Count;

            OnEnemyChanged?.Invoke();
        }

        public CharacterBase GetCurrentEnemy()
        {
            return CharacterPool.GetEnemyCharacters()[CurrentEnemyIndex];
        }

        public CharacterBase GetPlayerCharacter()
        {
            return CharacterPool.GetAlliedCharacters()[0];
        }

        public List<Skill> GetCurrentEnemySkillSet()
        {
            return GetCurrentEnemy().SkillSets[CurrentEnemySkillSetIndex].Skills;
        }

        public int GetTimelineLength()
        {
            return BattleBoard.EnemyTimeline.Length;
        }

        private void OnEnemyDied(CharacterBase Enemy)
        {
            OnBattleFinished?.Invoke(BattleResult.Win);
        }

        private void OnAllyDied(CharacterBase Ally) 
        {
            OnBattleFinished?.Invoke(BattleResult.Lose);
        }

        public void CreateBattleEquipment(Equipment EquipmentRef, EquipmentSlot Slot)
        {
            OnBattleEquipmentCreationCommited?.Invoke(EquipmentRef, Slot);
        }

        #region Timer
        public void ExecuteActionsAtPosition(int Position)
        {
            Action alliedAction = BattleBoard.AlliedTimeline.GetActionAtPosition(Position);
            Action enemyAction = BattleBoard.EnemyTimeline.GetActionAtPosition(Position);

            ActionData Data = ActionBehaviour.Execute(alliedAction, enemyAction);
            TimelineTimer.SetSpeed(Data.IsSignificant ? GameInstance.Get().MinTimelineSpeed : GameInstance.Get().MaxTimelineSpeed);

            OnActionExecuted?.Invoke(Data);
        }

        public void StartBattleTimer()
        {
            GameObject timerObject = new GameObject("Timer");
            TimelineTimer = timerObject.AddComponent<BattleTimelineTimer>();
            TimelineTimer.OnIntegerValue += (int Position) => OnTimerIntegerValue?.Invoke(Position);
            TimelineTimer.OnUpdateInterp += (float Position) => OnTimerInterpValue?.Invoke(Position);
            TimelineTimer.OnElapsed += () => OnTimerFinished?.Invoke();
            TimelineTimer.OnStopped += () => OnTimerFinished?.Invoke();
            TimelineTimer.Launch(1.0f, GetTimelineLength());

            OnTimerStarted?.Invoke();
        }

        public void StopBattleTimer()
        {
            TimelineTimer.Stop();
        }
        #endregion Timer
    }
}