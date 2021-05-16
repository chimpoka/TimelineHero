using UnityEngine;
using TimelineHero.Character;
using TimelineHero.Battle;

namespace TimelineHero.BattleView
{
    public class BoardView : MonoBehaviour
    {
        public System.Action OnTimelinesVisualCreated;

        public CharacterTimelineView EnemyTimeline;
        public AlliedCharacterTimelineView AlliedTimeline;
        public BattleTimelineTimerView TimerView;

        public void SetPlayBattleState()
        {
            AlliedTimeline.OnStartPlayState();
            EnemyTimeline.OnStartPlayState();
        }

        public void RegenerateTimelinesAndTimerView()
        {
            EnemyTimeline?.DestroyUiObject();
            AlliedTimeline?.DestroyUiObject();
            TimerView?.DestroyUiObject();

            // EnemyTimeline must be created first
            GenerateEnemiesTimeline();
            GenerateAlliedTimeline();
            CreateTimelineTimer();
        }

        private void GenerateEnemiesTimeline()
        {
            EnemyTimeline = GenerateTimeline(BattlePrefabsConfig.Instance.TimelinePrefab);
            EnemyTimeline.Initialize(BattleSystem.Get().BattleBoard.EnemyTimeline);

            foreach (Skill skill in BattleSystem.Get().GetCurrentEnemy().Skills)
            {
                CardWrapper cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardWrapperPrefab);
                cardWrapper.SetState(CardState.Hand, skill);
                EnemyTimeline.AddCard(cardWrapper, false);
            }

            EnemyTimeline.MaxLength = EnemyTimeline.Length;
            EnemyTimeline.Size = EnemyTimeline.GetContentSize();
            EnemyTimeline.GetTransform().localScale = new Vector3(1,-1,1);
        }

        private void GenerateAlliedTimeline()
        {
            AlliedTimeline = GenerateTimeline(BattlePrefabsConfig.Instance.AlliedTimelinePrefab) as AlliedCharacterTimelineView;
            AlliedTimeline.Initialize(BattleSystem.Get().BattleBoard.AlliedTimeline);
            AlliedTimeline.GetTransform().localScale = Vector3.one;
            AlliedTimeline.AnchoredPosition += new Vector2(0, -EnemyTimeline.Size.y);
            AlliedTimeline.Size = EnemyTimeline.Size;
            AlliedTimeline.MaxLength = EnemyTimeline.MaxLength;
        }

        private CharacterTimelineView GenerateTimeline(CharacterTimelineView Prefab)
        {
            CharacterTimelineView timeline = Instantiate(Prefab);
            timeline.GetTransform().SetParent(transform);
            timeline.GetTransform().localScale = Vector3.one;
            timeline.AnchoredPosition = new Vector2(0, timeline.Size.y / 2);

            return timeline;
        }

        private void CreateTimelineTimer()
        {
            TimerView = Instantiate(BattlePrefabsConfig.Instance.TimerPrefab);
            TimerView.GetTransform().SetParent(transform);
            TimerView.GetTransform().localScale = Vector3.one;

            Bounds bounds = EnemyTimeline.WorldBounds;
            Vector2 startPoint = new Vector2(bounds.min.x, bounds.max.y);
            Vector2 endPoint = new Vector2(bounds.max.x, bounds.max.y);
            TimerView.SetMovementLine(new Line(startPoint, endPoint));
            TimerView.WorldPosition = startPoint;
        }
    }
}