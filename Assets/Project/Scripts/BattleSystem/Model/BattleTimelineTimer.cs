using UnityEngine;

namespace TimelineHero.Battle
{
    public class BattleTimelineTimer : MonoBehaviour
    {
        // Returns interpolated value between 0 and 1
        public System.Action<float> OnUpdateInterp;
        // Returns accumulated time
        public System.Action<float> OnUpdate;
        // Returns action index
        public System.Action<int> OnIntegerValue;
        // On timer ends
        public System.Action OnElapsed;
        // On timer interrupted
        public System.Action OnStopped;

        private float AccumulatedTimeInSeconds = 0.0f;
        private int CurrentStep = -1;
        private float StepTimeInSeconds;
        private float EndTimeIsSeconds;
        private bool IsEnabled = false;
        private float Speed = 1.0f;

        public void Launch(float ActionsInSecond, int TimelineLength)
        {
            AccumulatedTimeInSeconds = 0.0f;
            StepTimeInSeconds = 1.0f / ActionsInSecond;
            EndTimeIsSeconds = TimelineLength / ActionsInSecond;
            IsEnabled = true;
        }

        public void Stop()
        {
            OnStopped?.Invoke();
            Destroy(gameObject);
        }

        public void SetSpeed(float Speed)
        {
            this.Speed = Speed;
        }

        public void Pause()
        {
            IsEnabled = false;
        }

        public void Continue()
        {
            IsEnabled = true;
        }

        private void FixedUpdate()
        {
            if (!IsEnabled)
                return;

            AccumulatedTimeInSeconds += Time.fixedDeltaTime * Speed;

            OnUpdate?.Invoke(AccumulatedTimeInSeconds);
            OnUpdateInterp?.Invoke(AccumulatedTimeInSeconds / EndTimeIsSeconds);

            if ((int)(AccumulatedTimeInSeconds / StepTimeInSeconds) > CurrentStep)
            {
                OnIntegerValue?.Invoke(++CurrentStep);
            }

            if (AccumulatedTimeInSeconds >= EndTimeIsSeconds)
            {
                OnElapsed?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}