﻿using System.Collections.Generic;
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

    public enum CardLocationType { NoParent, Hand, Board, Deck }

    public class Card : UiComponent, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler 
    {
        [SerializeField]
        private RectTransform DelimeterPrefab;
        [SerializeField]
        private StepPrefabStruct[] Prefabs;

        public int Length { get => SkillCached.Length; }

        public delegate void SkillEventHandler(Card skill, PointerEventData eventData);
        public event SkillEventHandler OnPointerDownEvent;
        public event SkillEventHandler OnPointerUpEvent;
        public event SkillEventHandler OnBeginDragEvent;
        public event SkillEventHandler OnEndDragEvent;
        public event SkillEventHandler OnDragEvent;

        public CardLocationType LocationType;

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

        static public float GetCardStaticHeight()
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
            CreateDelimeter();
        }

        public int GetLength()
        {
            return SkillCached.Length;
        }

        public Skill GetSkill()
        {
            return SkillCached;
        }

        private void CreateSteps(int Length)
        {
            Steps = new List<TimelineStepView>();

            for (int i = 0; i < Length; ++i)
            {
                Action action = SkillCached.GetActionInPosition(i);
                action = action ?? new Action(CharacterActionType.Empty, i, SkillCached.Owner);

                TimelineStepView step = Instantiate(PrefabsDictionary[action.ActionType]);
                step.SetParent(GetTransform());
                step.AnchoredPosition = new Vector2(i * step.Size.x, 0);
                step.SetValue(action.Value);

                Steps.Add(step);
            }

            Size = GetTimelineStepStaticSize() * new Vector2(Length, 1.0f);
        }

        private void CreateDelimeter()
        {
            RectTransform delimeter = Instantiate(DelimeterPrefab);
            delimeter.SetParent(GetTransform());
            delimeter.localScale = Vector3.one;
            delimeter.anchoredPosition = Vector2.zero;
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