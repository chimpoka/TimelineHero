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
            GenerateEnemiesTimeline();
        }

        void Update()
        {

        }

        public void SetEnemies(List<CharacterBase> NewEnemies)
        {
            Enemies = NewEnemies;
        }

        private void GenerateEnemiesTimeline()
        {
            EnemyTimeline = Instantiate(BattlePrefabsConfig.Instance.CharacterTimelinePrefab);

            RectTransform timelineTransform = EnemyTimeline.GetComponent<RectTransform>();
            timelineTransform.SetParent(transform);
            timelineTransform.localScale = Vector3.one;
            timelineTransform.anchoredPosition = new Vector2(0, timelineTransform.sizeDelta.y / 2);

            foreach (Skill skill in Enemies[0].Skills)
            {
                SkillView skillView = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
                skillView.SetSkill(skill);

                EnemyTimeline.AddSkill(skillView);
            }
        }
    }
}