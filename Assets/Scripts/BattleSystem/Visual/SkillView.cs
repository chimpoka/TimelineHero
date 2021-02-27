using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimelineHero.Character;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    [System.Serializable]
    public struct StepPrefabStruct
    {
        public CharacterActionType ActionType;
        public TimelineStepView Prefab;
    }

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
        static private Dictionary<CharacterActionType, TimelineStepView> PrefabsDictionary;


        private void Awake()
        {
            PrefabsDictionary = new Dictionary<CharacterActionType, TimelineStepView>();
            foreach (StepPrefabStruct prefab in Prefabs)
            {
                PrefabsDictionary.Add(prefab.ActionType, prefab.Prefab);
            }
        }

        static public float GetSkillStaticHeight()
        {
            return GetTimelineStepStaticSize().y;
        }

        static public Vector2 GetTimelineStepStaticSize()
        {
            Rect prefabRect = PrefabsDictionary[CharacterActionType.Empty].GetComponent<RectTransform>().rect;

            return new Vector2(prefabRect.width, prefabRect.height);
        }

        public void SetSkill(Skill NewSkill)
        {
            SkillCached = NewSkill;
            CreateSteps(SkillCached.Length);
        }

        public int GetLength()
        {
            return SkillCached.Length;
        }

        private void CreateSteps(int Length)
        {
            Steps = new List<TimelineStepView>();

            for (int i = 0; i < Length; ++i)
            {
                Action action = SkillCached.GetActionInPosition(i);
                action = action ?? new Action(CharacterActionType.Empty, i, SkillCached.Owner);

                TimelineStepView step = Instantiate(PrefabsDictionary[action.ActionType]);
                step.GetTransform().SetParent(GetTransform());
                step.AnchoredPosition = new Vector2(i * step.Size.x, 0);

                Steps.Add(step);
            }

            Size = GetTimelineStepStaticSize() * new Vector2(Length, 1.0f);
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
}