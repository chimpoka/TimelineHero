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
        private BattleSystem BattleSystemCached;
        private Line MovementLine;

        public void SetBattleSystem(BattleSystem NewBattleSystem)
        {
            BattleSystemCached = NewBattleSystem;
            BattleSystemCached.OnTimerStarted += OnTimerStarted;
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
            BattleSystemCached.GetTimer().OnUpdateInterp += OnTimerUpdate;
        }

        private void OnTimerUpdate(float InterpValue)
        {
            WorldPosition = Vector2.Lerp(MovementLine.StartPoint, MovementLine.EndPoint, InterpValue);
        }
    }
}