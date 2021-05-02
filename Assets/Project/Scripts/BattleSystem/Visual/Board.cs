using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    public class Board : MonoBehaviour
    {
        public System.Action OnTimelinesCreated;
        public System.Action OnAlliedTimelineLengthChanged;

        private CharacterTimeline EnemyTimeline;
        private AlliedCharacterTimeline AlliedTimeline;
        private BattleTimelineTimerView TimerView;

        public void Initialize()
        {
            BattleSystem.Get().OnTimerIntegerValue += ExecuteActionsInPosition;
            OnTimelinesCreated += SubscribeOnTimelineEvents;
        }

        private void SubscribeOnTimelineEvents()
        {
            AlliedTimeline.OnLengthChanged += OnAlliedTimelineLengthChanged;
        }

        public BattleTimelineTimerView GetTimerView()
        {
            return TimerView;
        }

        public int GetTimelineLength()
        {
            return GetEnemyTimeline().Length;
        }

        public AlliedCharacterTimeline GetAlliedTimeline()
        {
            return AlliedTimeline;
        }

        public CharacterTimeline GetEnemyTimeline()
        {
            return EnemyTimeline;
        }

        public void ExecuteActionsInPosition(int Position)
        {
            Action alliedAction = GetAlliedTimeline().GetActionInPosition(Position);
            Action enemyAction = GetEnemyTimeline().GetActionInPosition(Position);
            BattleSystem.Get().ExecuteActions(alliedAction, enemyAction);
        }

        public void OnStartConstructState()
        {
            CreateNextEnemy();
        }

        public void OnStartPlayState()
        {
            GetAlliedTimeline().OnStartPlayState();
            GetEnemyTimeline().OnStartPlayState();
        }

        public void RegenerateBoard()
        {
            List<CardWrapper> alliedCards = null;

            if (AlliedTimeline)
            {
                alliedCards = AlliedTimeline.RemoveCardsFromTimeline();
                AlliedTimeline.DestroyUiObject();
            }

            EnemyTimeline?.DestroyUiObject();
            TimerView?.DestroyUiObject();

            EnemyTimeline = GenerateEnemiesTimeline();
            AlliedTimeline = GenerateAlliedTimeline(GetEnemyTimeline().Size, GetEnemyTimeline().MaxLength);

            OnTimelinesCreated?.Invoke();

            if (alliedCards != null)
            {
                BattleSceneController scene = (BattleSceneController)SceneControllerBase.Instance;
                foreach (CardWrapper card in alliedCards)
                {
                    if (!AlliedTimeline.TryAddCard(card))
                    {
                        scene.BattleView.PlayerHand.AddCard(card);
                    }
                }
            }
        }

        public void CreateNextEnemy()
        {
            List<CharacterBase> enemies = BattleSystem.Get().GetEnemyCharacters();
            BattleSystem.Get().CurrentEnemyIndex = (BattleSystem.Get().CurrentEnemyIndex + 1) % enemies.Count;
            RegenerateBoard();
        }

        public void CreatePreviousEnemy()
        {
            List<CharacterBase> enemies = BattleSystem.Get().GetEnemyCharacters();
            BattleSystem.Get().CurrentEnemyIndex = (BattleSystem.Get().CurrentEnemyIndex + enemies.Count - 1) % enemies.Count;
            RegenerateBoard();
        }

        private List<Skill> GetEnemySkills()
        {
            return BattleSystem.Get().GetCurrentEnemy().Skills;
        }

        private CharacterTimeline GenerateEnemiesTimeline()
        {
            CharacterTimeline enemyTimeline = GenerateTimeline(BattlePrefabsConfig.Instance.TimelinePrefab);

            foreach (Skill skill in GetEnemySkills())
            {
                CardWrapper cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardWrapperPrefab);
                cardWrapper.SetState(CardState.Hand, skill);
                enemyTimeline.AddCard(cardWrapper, false);
            }

            enemyTimeline.MaxLength = enemyTimeline.Length;
            enemyTimeline.Size = enemyTimeline.GetContentSize();
            enemyTimeline.GetTransform().localScale = new Vector3(1,-1,1);

            enemyTimeline.OnConstruct();

            Bounds bounds = enemyTimeline.WorldBounds;
            Vector2 startPoint = new Vector2(bounds.min.x, bounds.max.y);
            Vector2 endPoint = new Vector2(bounds.max.x, bounds.max.y);
            CreateTimelineTimer(new Line(startPoint, endPoint));

            return enemyTimeline;
        }

        private AlliedCharacterTimeline GenerateAlliedTimeline(Vector2 EnemyTimelineSize, int MaxLength)
        {
            AlliedCharacterTimeline alliedTimeline = GenerateTimeline(BattlePrefabsConfig.Instance.AlliedTimelinePrefab) as AlliedCharacterTimeline;
            alliedTimeline.GetTransform().localScale = Vector3.one;
            alliedTimeline.AnchoredPosition += new Vector2(0, -EnemyTimelineSize.y);
            alliedTimeline.Size = EnemyTimelineSize;
            alliedTimeline.MaxLength = MaxLength;

            alliedTimeline.OnConstruct();

            return alliedTimeline;
        }

        private CharacterTimeline GenerateTimeline(CharacterTimeline Prefab)
        {
            CharacterTimeline timeline = Instantiate(Prefab);
            timeline.GetTransform().SetParent(transform);
            timeline.GetTransform().localScale = Vector3.one;
            timeline.AnchoredPosition = new Vector2(0, timeline.Size.y / 2);

            return timeline;
        }

        private void CreateTimelineTimer(Line MovementLine)
        {
            TimerView = Instantiate(BattlePrefabsConfig.Instance.TimerPrefab);
            TimerView.GetTransform().SetParent(transform);
            TimerView.GetTransform().localScale = Vector3.one;
            TimerView.SetMovementLine(MovementLine);
            TimerView.WorldPosition = MovementLine.StartPoint;
        }
    }
}