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

    public class Card : UiComponent
    {
        [SerializeField]
        private RectTransform DelimeterPrefab;
        [SerializeField]
        private StepPrefabStruct[] Prefabs;

        public int Length { get => SkillCached.Length; }

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
            CreateSteps();
            CreateDelimeter();
        }

        public Skill GetSkill()
        {
            return SkillCached;
        }

        private void CreateSteps()
        {
            Steps = new List<TimelineStepView>();

            for (int i = 0; i < SkillCached.Length; ++i)
            {
                Action action = SkillCached.GetActionInPosition(i);
                action = action ?? new Action(CharacterActionType.Empty, i, SkillCached.Owner);

                TimelineStepView step = Instantiate(PrefabsDictionary[action.ActionType]);
                step.SetParent(GetTransform());
                step.AnchoredPosition = new Vector2(i * GetTimelineStepStaticSize().x, 0);
                step.SetValue(action.Value);

                Steps.Add(step);
            }

            Size = GetTimelineStepStaticSize() * new Vector2(SkillCached.VirtualLength, 1.0f);
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