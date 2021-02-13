using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
using UnityEngine.EventSystems;

namespace TimelineHero.Battle
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField]
        private TimelineStepView StepPrefab;

        Skill SkillCached;
        List<TimelineStepView> Steps;

        void Start()
        {
            CreateSteps(5);
        }

        void Update()
        {

        }

        void CreateSteps(int Length)
        {
            Steps = new List<TimelineStepView>();

            for (int i = 0; i < Length; ++i)
            {
                TimelineStepView step = Instantiate(StepPrefab);
                Transform stepTransform = step.GetComponent<Transform>();
                step.Create();

                step.StepSpriteRenderer.size = (new Vector2(0.3f, 1.0f));

                Vector3 newPosition = new Vector3(i * step.StepSpriteRenderer.size.x, 0, 0);
                stepTransform.localPosition = newPosition;

                Steps.Add(step);

                stepTransform.SetParent(transform);
            }

            Vector2 spriteSize = StepPrefab.GetSize();
            
            Vector2 colliderSize = new Vector2(spriteSize.x * Length, spriteSize.y);
            Vector2 colliderOffset = new Vector2(colliderSize.x / 2 - spriteSize.x / 2, 0);

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = colliderSize;
            collider.offset = colliderOffset;
        }

        private void OnMouseDown()
        {
            print("Down");
        }
    }
}