using UnityEngine;

namespace TimelineHero.Battle
{
    public class BattleTimelineTimer : MonoBehaviour
    {
        public void Launch(float ActionsInSecond, int TimelineLength)
        {
            AccumulatedTimeInSeconds = 0.0f;
            StepTimeInSeconds = 1.0f / ActionsInSecond;
            EndTimeIsSeconds = TimelineLength / ActionsInSecond;
            IsEnabled = true;
        }

        // Returns interpolated value between 0 and 1
        public System.Action<float> OnUpdateInterp;
        // Returns accumulated time
        public System.Action<float> OnUpdate;
        // Returns accumulated action
        public System.Action<int> OnActionExecuted;
        // On timer ends
        public System.Action OnElapsed;

        private float AccumulatedTimeInSeconds = 0.0f;
        private int CurrentStep = -1;
        private float StepTimeInSeconds;
        private float EndTimeIsSeconds;
        private bool IsEnabled = false;


        private void FixedUpdate()
        {
            if (!IsEnabled)
                return;

            AccumulatedTimeInSeconds += Time.fixedDeltaTime;

            OnUpdate?.Invoke(AccumulatedTimeInSeconds);
            OnUpdateInterp?.Invoke(AccumulatedTimeInSeconds / EndTimeIsSeconds);

            if (AccumulatedTimeInSeconds / StepTimeInSeconds >= CurrentStep)
            {
                OnActionExecuted?.Invoke(++CurrentStep);
            }

            if (AccumulatedTimeInSeconds >= EndTimeIsSeconds)
            {
                OnElapsed?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}