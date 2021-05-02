using System.Collections;
using System.Collections.Generic;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.Battle
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
            BattleSystem.Get().OnTimerStarted += OnTimerStarted;
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
            BattleSystem.Get().OnTimerInterpValue += OnTimerUpdate;
        }

        private void OnTimerUpdate(float InterpValue)
        {
            WorldPosition = Vector2.Lerp(MovementLine.StartPoint, MovementLine.EndPoint, InterpValue);
        }

        private void OnDestroy()
        {
            BattleSystem.Get().OnTimerStarted -= OnTimerStarted;
            BattleSystem.Get().OnTimerInterpValue -= OnTimerUpdate;
        }
    }
}