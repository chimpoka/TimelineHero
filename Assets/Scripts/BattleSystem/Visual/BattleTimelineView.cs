using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class BattleTimelineView : MonoBehaviour
    {
        List<CharacterBase> Enemies;
        CharacterTimelineView EnemyTimeline;
        CharacterTimelineView AlliedTimeline;

        private void Awake()
        {

        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetEnemies(List<CharacterBase> NewEnemies)
        {
            Enemies = NewEnemies;
        }

        public ref CharacterTimelineView GetAlliedTimeline()
        {
            return ref AlliedTimeline;
        }

        public void GenerateEnemiesTimeline()
        {
            EnemyTimeline = GenerateTimeline();

            foreach (Skill skill in Enemies[0].Skills)
            {
                SkillView skillView = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
                skillView.SetSkill(skill);

                EnemyTimeline.AddSkill(skillView);
            }

            EnemyTimeline.Size = EnemyTimeline.GetContentSize();
        }

        public void GenerateAlliedTimeline()
        {
            AlliedTimeline = GenerateTimeline();
            RectTransform alliedTimelineTransform = AlliedTimeline.GetComponent<RectTransform>();
            alliedTimelineTransform.localScale = Vector3.one;
            alliedTimelineTransform.anchoredPosition += new Vector2(0, -EnemyTimeline.GetContentSize().y);
            AlliedTimeline.Size = EnemyTimeline.GetContentSize();
        }

        private CharacterTimelineView GenerateTimeline()
        {
            CharacterTimelineView timeline = Instantiate(BattlePrefabsConfig.Instance.CharacterTimelinePrefab);

            RectTransform timelineTransform = timeline.GetComponent<RectTransform>();
            timelineTransform.SetParent(transform);
            timelineTransform.localScale = Vector3.one;
            timelineTransform.anchoredPosition = new Vector2(0, timelineTransform.sizeDelta.y / 2);

            return timeline;
        }
    }
}