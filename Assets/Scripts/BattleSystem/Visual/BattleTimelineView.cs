using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class BattleTimelineView : MonoBehaviour
    {
        private List<CharacterBase> Enemies;
        private CharacterTimelineView EnemyTimeline;
        private CharacterTimelineView AlliedTimeline;

        public void SetEnemies(List<CharacterBase> NewEnemies)
        {
            Enemies = NewEnemies;
        }

        public CharacterTimelineView GetAlliedTimeline()
        {
            return AlliedTimeline;
        }

        public CharacterTimelineView GetEnemyTimeline()
        {
            return EnemyTimeline;
        }

        public void GenerateView()
        {
            GenerateEnemiesTimeline();
            GenerateAlliedTimeline();
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
            AlliedTimeline.GetTransform().localScale = Vector3.one;
            AlliedTimeline.AnchoredPosition += new Vector2(0, -EnemyTimeline.GetContentSize().y);
            AlliedTimeline.Size = EnemyTimeline.GetContentSize();
        }

        private CharacterTimelineView GenerateTimeline()
        {
            CharacterTimelineView timeline = Instantiate(BattlePrefabsConfig.Instance.CharacterTimelinePrefab);
            timeline.GetTransform().SetParent(transform);
            timeline.GetTransform().localScale = Vector3.one;
            timeline.AnchoredPosition = new Vector2(0, timeline.Size.y / 2);

            return timeline;
        }
    }
}