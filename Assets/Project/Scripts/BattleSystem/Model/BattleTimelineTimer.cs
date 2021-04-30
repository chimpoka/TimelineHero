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

        private void FixedUpdate()
        {
            if (!IsEnabled)
                return;

            AccumulatedTimeInSeconds += Time.fixedDeltaTime;

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