using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public enum BattleResult { Lose, Win }

    public class BattleSystem : Core.Subsystem<BattleSystem>
    {
        public System.Action OnTimerStarted;
        public System.Action OnTimerFinished;
        public System.Action<int> OnTimerIntegerValue;
        public System.Action<float> OnTimerInterpValue;

        public System.Action<ActionData> OnActionExecuted;
        public System.Action<BattleResult> OnBattleFinished;

        public System.Action<List<Skill>> OnDrawCards;
        public System.Action OnDiscardAllCardsFromTimeline;
        public System.Action OnDiscardCardsFromDiscardSection;
        public System.Action OnEnemyChanged;

        public System.Action OnInitialBattleState;
        public System.Action OnMainBattleState;
        public System.Action OnPlayBattleState;

        public int CurrentEnemyIndex = 0;
        public int CurrentEnemySkillSetIndex = -1;

        public Hand PlayerHand = new Hand();
        public Board BattleBoard = new Board();
        public DrawDeck PlayerDrawDeck = new DrawDeck();
        public DiscardDeck PlayerDiscardDeck = new DiscardDeck();
        public DiscardSection PlayerDiscardSection = new DiscardSection();

        private BattleTimelineTimer TimelineTimer;
        private ActionExecutionBehaviour ActionBehaviour = new ActionExecutionBehaviour();
        private List<CharacterBase> AlliedCharacters = new List<CharacterBase>();
        private List<CharacterBase> EnemyCharacters = new List<CharacterBase>();

        protected override void OnInitialize()
        {
            PlayerDrawDeck.AddSkills(GetAlliedCharacters().SelectMany(character => character.Skills).ToList());

            foreach (CharacterBase enemy in GetEnemyCharacters())
            {
                enemy.OnDied += OnEnemyDied;
            }

            foreach (CharacterBase ally in GetAlliedCharacters())
            {
                ally.OnDied += OnAllyDied;
            }

            OnTimerIntegerValue += ExecuteActionsAtPosition;
        }

        public void SetInitialBattleState()
        {
            DiscardAllCardsFromTimeline();
            DrawCards(GameInstance.Get().DrawCardCount);
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

        public void DrawCards(int Count)
        {
            List<Skill> skills = PlayerDrawDeck.Draw(Count);

            if (skills == null)
                return;

            if (skills.Count < Count)
            {
                ShuffleDiscardDeckToDrawDeck();
                skills.AddRange(PlayerDrawDeck.Draw(Count - skills.Count));
            }

            OnDrawCards?.Invoke(skills);
        }

        public void ShuffleDiscardDeckToDrawDeck()
        {
            PlayerDrawDeck.AddSkills(PlayerDiscardDeck.RemoveAllSkills());
        }

        public void DiscardAllCardsFromTimeline()
        {
            PlayerDiscardDeck.AddSkills(BattleBoard.AlliedTimeline.RemoveAllSkills());

            OnDiscardAllCardsFromTimeline?.Invoke();
        }

        public void DiscardCardsFromDiscardSection()
        {
            PlayerDiscardDeck.AddSkills(PlayerDiscardSection.RemoveAllSkills());

            OnDiscardCardsFromDiscardSection?.Invoke();
        }

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
            List<CharacterBase> enemies = GetEnemyCharacters();
            CurrentEnemySkillSetIndex = 0;
            CurrentEnemyIndex = (CurrentEnemyIndex + 1) % enemies.Count;

            OnEnemyChanged?.Invoke();
        }

        public void CreatePreviousEnemy()
        {
            List<CharacterBase> enemies = GetEnemyCharacters();
            CurrentEnemySkillSetIndex = 0;
            CurrentEnemyIndex = (CurrentEnemyIndex + enemies.Count - 1) % enemies.Count;

            OnEnemyChanged?.Invoke();
        }

        public List<CharacterBase> GetAlliedCharacters()
        {
            if (AlliedCharacters.Count == 0)
            {
                AlliedCharacters = GameInstance.Get().GetAllies();
            }
            return AlliedCharacters;
        }

        public List<CharacterBase> GetEnemyCharacters()
        {
            if (EnemyCharacters.Count == 0)
            {
                EnemyCharacters = GameInstance.Get().GetEnemies();
            }
            return EnemyCharacters;
        }

        public CharacterBase GetCurrentEnemy()
        {
            return GetEnemyCharacters()[CurrentEnemyIndex];
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
            foreach (CharacterBase character in GetAlliedCharacters())
            {
                if (!character.IsDead)
                {
                    return;
                }
            }

            OnBattleFinished?.Invoke(BattleResult.Lose);
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