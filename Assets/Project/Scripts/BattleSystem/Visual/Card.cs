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

        public int Length { get => SkillCached.Length; }

        [HideInInspector]
        public List<TimelineStepView> Steps = new List<TimelineStepView>();

        private Skill SkillCached;

        static private Dictionary<CharacterActionType, TimelineStepView> PrefabsDictionary;

        private void Awake()
        {
            // TODO: Remove    private StepPrefabStruct[] Prefabs;

            PrefabsDictionary = new Dictionary<CharacterActionType, TimelineStepView>();
            foreach (StepPrefabStruct prefab in Prefabs)
            {
                PrefabsDictionary.Add(prefab.ActionType, prefab.Prefab);
            }

            CreateDelimeter();
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
        }

        public Skill GetSkill()
        {
            return SkillCached;
        }

        public void PlayDestroyAnimation()
        {
            float duration = 5.0f;

            foreach (TimelineStepView step in Steps)
            {
                if (step.DisabledInPlayState)
                {
                    step.PlayDestroyAnimation(duration);
                }
            }
        }

        public void PlayRestoreAnimation()
        {
            float duration = 2.0f;

            foreach (TimelineStepView step in Steps)
            {
                if (step.DisabledInPlayState)
                {
                    step.PlayRestoreAnimation(duration);
                }
            }
        }

        private void CreateSteps()
        {
            for (int i = 0; i < SkillCached.Length; ++i)
            {
                Action action = SkillCached.GetActionInPosition(i);
                action = action ?? new Action(CharacterActionType.Empty, i, SkillCached.Owner);

                if (Steps.Count > i && Steps[i].ActionCached != null && Steps[i].ActionCached.Equals(action))
                {
                    //Steps[i].SetValue(action.Value);
                    continue;
                }

                TimelineStepView step = Instantiate(PrefabsDictionary[action.ActionType]);
                step.SetParent(GetTransform());
                step.AnchoredPosition = new Vector2(i * GetTimelineStepStaticSize().x, 0);
                step.SetValue(action.Value);
                step.DisabledInPlayState = action.DisabledInPlayState;
                step.ActionCached = action;

                if (i < Steps.Count)
                {
                    ReplaceWith(Steps[i], step);
                }
                else
                {
                    Steps.Add(step);
                }
            }

            for (int i = Steps.Count - 1; i >= SkillCached.Length; --i)
            {
                RemoveStepAt(i);
            }

            Size = GetTimelineStepStaticSize() * new Vector2(SkillCached.Length, 1.0f);
        }

        private void RemoveStepAt(int index)
        {
            if (index >= Steps.Count)
                return;

            Steps[index].DestroyUiObject();
            Steps.RemoveAt(index);
        }

        private void ReplaceWith(TimelineStepView FromStep, TimelineStepView ToStep)
        {
            int index = Steps.IndexOf(FromStep);
            Steps[index].DestroyUiObject();
            Steps[index] = ToStep;
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