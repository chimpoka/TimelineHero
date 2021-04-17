using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using DG.Tweening;

namespace TimelineHero.Battle
{
    [System.Serializable]
    public struct StepPrefabStruct
    {
        public CharacterActionType ActionType;
        public TimelineStepView Prefab;
    }

    public class Card : UiComponent
    {
        [SerializeField]
        private RectTransform DelimeterPrefab;
        [SerializeField]
        private StepPrefabStruct[] Prefabs;

        public int Length { get => SkillCached.HandLength; }

        private Skill SkillCached;
        public List<TimelineStepView> Steps;

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
            CreateSteps();
            CreateDelimeter();
        }

        public Skill GetSkill()
        {
            return SkillCached;
        }

        public void PlayAnimation()
        {
            float duration = 5.0f;

            foreach (TimelineStepView step in Steps)
            {
                if (step.DisabledInPlayState)
                {
                    step.PlayAnimation(duration);
                }
            }

            DOTween.To(() => 0, x => { }, 1, duration).onComplete += DestroyUiObject;
        }

        public void PlayAnimationAtPosition()
        {

        }

        private void CreateSteps()
        {
            Steps = new List<TimelineStepView>();

            for (int i = 0; i < SkillCached.HandLength; ++i)
            {
                Action action = SkillCached.GetActionInPosition(i);
                action = action ?? new Action(CharacterActionType.Empty, i, SkillCached.Owner);

                TimelineStepView step = Instantiate(PrefabsDictionary[action.ActionType]);
                step.SetParent(GetTransform());
                step.AnchoredPosition = new Vector2(i * GetTimelineStepStaticSize().x, 0);
                step.SetValue(action.Value);
                step.DisabledInPlayState = action.DisabledInPlayState;

                Steps.Add(step);
            }

            Size = GetTimelineStepStaticSize() * new Vector2(SkillCached.BoardLength, 1.0f);
        }

        private void CreateDelimeter()
        {
            RectTransform delimeter = Instantiate(DelimeterPrefab);
            delimeter.SetParent(GetTransform());
            delimeter.localScale = Vector3.one;
            delimeter.anchoredPosition = Vector2.zero;
        }
    }
}