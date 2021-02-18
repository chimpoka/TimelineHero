using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimelineHero.Character;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public enum SkillParentObject { NoParent, Container, Timeline }

    public struct SkillRelationData
    {
        public Vector2 PositionInContainer;
        public Vector2 PositionInTimeline;
        public SkillParentObject Parent;

        public void Clear()
        {
            PositionInContainer = Vector2.zero;
            PositionInTimeline = Vector2.zero;
        }
    }

    public class SkillView : UiComponent, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler 
    {
        [SerializeField]
        private TimelineStepView StepPrefab;
        [SerializeField]
        private StepPrefabStruct[] Prefabs;

        public delegate void SkillEventHandler(SkillView skill, PointerEventData eventData);
        public event SkillEventHandler OnPointerDownEvent;
        public event SkillEventHandler OnPointerUpEvent;
        public event SkillEventHandler OnBeginDragEvent;
        public event SkillEventHandler OnEndDragEvent;
        public event SkillEventHandler OnDragEvent;

        public SkillRelationData RelationData;

        private Skill SkillCached;
        private List<TimelineStepView> Steps;
        private Dictionary<CharacterActionType, TimelineStepView> PrefabsDictionary;
        private Vector2 position;
        private Vector2 size;


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

        public Vector2 GetCenterPosition()
        {
            return AnchoredPosition;// + TransformCached.rect.center;
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

            Size = new Vector2(StepPrefab.GetSize().x * Length, StepPrefab.GetSize().y);
        }

        #region SkillEvents
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(this, eventData);
        }
        #endregion SkillEvents
    }

    [System.Serializable]
    public struct StepPrefabStruct
    {
        public CharacterActionType ActionType;
        public TimelineStepView Prefab;
    }
}