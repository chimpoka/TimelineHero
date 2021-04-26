using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class Board : MonoBehaviour
    {
        private BattleSystem BattleSystemCached;
        private CharacterTimeline EnemyTimeline;
        private AlliedCharacterTimeline AlliedTimeline;
        private BattleTimelineTimerView TimerView;

        public void Initialize(BattleSystem BattleSystemRef)
        {
            BattleSystemCached = BattleSystemRef;
            BattleSystemCached.OnTimerIntegerValue += ExecuteActionsInPosition;
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
            AlliedTimeline = AlliedTimeline ?? GenerateAlliedTimeline(GetEnemyTimeline().Size, GetEnemyTimeline().MaxLength);
            return AlliedTimeline;
        }

        public CharacterTimeline GetEnemyTimeline()
        {
            EnemyTimeline = EnemyTimeline ?? GenerateEnemiesTimeline();
            return EnemyTimeline;
        }

        public void ExecuteActionsInPosition(int Position)
        {
            Action alliedAction = GetAlliedTimeline().GetActionInPosition(Position);
            Action enemyAction = GetEnemyTimeline().GetActionInPosition(Position);
            BattleSystemCached.ExecuteActions(alliedAction, enemyAction);
        }

        public void OnStartConstructState()
        {
            GetEnemyTimeline().RebuildPreBattleCards();
        }

        public void OnStartPlayState()
        {
            GetAlliedTimeline().OnStartPlayState();
            GetEnemyTimeline().OnStartPlayState();
        }

        private List<Skill> GetEnemySkills()
        {
            return BattleSystemCached.GetEnemyCharacters()[0].Skills;
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
            TimerView.SetBattleSystem(BattleSystemCached);
            TimerView.WorldPosition = MovementLine.StartPoint;
        }
    }
}