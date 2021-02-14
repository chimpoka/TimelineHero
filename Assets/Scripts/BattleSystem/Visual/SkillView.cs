using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class SkillView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private TimelineStepView StepPrefab;
        [SerializeField]
        private StepPrefabStruct[] Prefabs;

        Skill SkillCached;
        List<TimelineStepView> Steps;
        public Dictionary<CharacterActionType, TimelineStepView> PrefabsDictionary;

        private void Awake()
        {
            PrefabsDictionary = new Dictionary<CharacterActionType, TimelineStepView>();
            foreach (StepPrefabStruct prefab in Prefabs)
            {
                PrefabsDictionary.Add(prefab.ActionType, prefab.Prefab);
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetSkill(Skill NewSkill)
        {
            SkillCached = NewSkill;
            CreateSteps(SkillCached.Length);
        }

        void CreateSteps(int Length)
        {
            Steps = new List<TimelineStepView>();

            for (int i = 0; i < Length; ++i)
            {
                Action action = SkillCached.GetActionInPosition(i);
                action = action ?? new Action(CharacterActionType.Empty, i);

                TimelineStepView step = Instantiate(PrefabsDictionary[action.ActionType]);

                RectTransform stepTransform = step.GetComponent<RectTransform>();
                stepTransform.SetParent(transform);
                stepTransform.localScale = Vector3.one;
                stepTransform.anchoredPosition = new Vector2(i * stepTransform.rect.width, 0);

                Steps.Add(step);
            }

            RectTransform skillTransform = GetComponent<RectTransform>();
            skillTransform.sizeDelta = new Vector2(StepPrefab.GetSize().x * Length, StepPrefab.GetSize().y);
        }

        public Vector2 GetSize()
        {
            return GetComponent<RectTransform>().sizeDelta;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            print("OnPointerDown");
        }
    }

    [System.Serializable]
    public struct StepPrefabStruct
    {
        public CharacterActionType ActionType;
        public TimelineStepView Prefab;
    }
}