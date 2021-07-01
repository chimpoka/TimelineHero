using TimelineHero.Battle;
using TimelineHero.Battle_v2;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public struct Line
    {
        public Line(Vector2 StartPoint, Vector2 EndPoint)
        {
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;
        }

        public Vector2 StartPoint;
        public Vector2 EndPoint;
    }

    public class BattleTimelineTimerView : UiComponent
    {
        private Line MovementLine;

        private void Awake()
        {
            BattleSystem_v2.Get().OnTimerStarted += OnTimerStarted;
        }

        public void SetMovementLine(Line NewMovementLine)
        {
            MovementLine = NewMovementLine;
        }
        
        public void ResetPosition()
        {
            WorldPosition = MovementLine.StartPoint;
        }

        private void OnTimerStarted()
        {
            BattleSystem_v2.Get().OnTimerInterpValue += OnTimerUpdate;
        }

        private void OnTimerUpdate(float InterpValue)
        {
            WorldPosition = Vector2.Lerp(MovementLine.StartPoint, MovementLine.EndPoint, InterpValue);
        }

        private void OnDestroy()
        {
            BattleSystem_v2.Get().OnTimerStarted -= OnTimerStarted;
            BattleSystem_v2.Get().OnTimerInterpValue -= OnTimerUpdate;
        }
    }
}