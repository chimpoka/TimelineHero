﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class BattleTimelineView : MonoBehaviour
    {
        private BattleSystem BattleSystemCached;
        private CharacterTimelineView EnemyTimeline;
        private CharacterTimelineView AlliedTimeline;
        private BattleTimelineTimerView TimerView;

        public void SetBattleSystem(BattleSystem NewBattleSystem)
        {
            BattleSystemCached = NewBattleSystem;
        }

        public BattleTimelineTimerView GetTimerView()
        {
            return TimerView;
        }

        public int GetTimelineLength()
        {
            return GetEnemyTimeline().GetLength();
        }

        public CharacterTimelineView GetAlliedTimeline()
        {
            AlliedTimeline = AlliedTimeline ?? GenerateAlliedTimeline(GetEnemyTimeline().Size);
            return AlliedTimeline;
        }

        public CharacterTimelineView GetEnemyTimeline()
        {
            EnemyTimeline = EnemyTimeline ?? GenerateEnemiesTimeline();
            return EnemyTimeline;
        }

        private List<Skill> GetEnemySkills()
        {
            return BattleSystemCached.GetEnemyCharacters()[0].Skills;
        }

        private CharacterTimelineView GenerateEnemiesTimeline()
        {
            CharacterTimelineView enemyTimeline = GenerateTimeline();

            foreach (Skill skill in GetEnemySkills())
            {
                SkillView skillView = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
                skillView.SetSkill(skill);
                enemyTimeline.AddSkill(skillView);
            }

            enemyTimeline.Size = enemyTimeline.GetContentSize();
            enemyTimeline.GetTransform().localScale = new Vector3(1,-1,1);

            Bounds bounds = enemyTimeline.WorldBounds;
            Vector2 startPoint = new Vector2(bounds.min.x, bounds.max.y);
            Vector2 endPoint = new Vector2(bounds.max.x, bounds.max.y);
            CreateTimelineTimer(new Line(startPoint, endPoint));

            return enemyTimeline;
        }

        private CharacterTimelineView GenerateAlliedTimeline(Vector2 EnemyTimelineSize)
        {
            CharacterTimelineView alliedTimeline = GenerateTimeline();
            alliedTimeline.GetTransform().localScale = Vector3.one;
            alliedTimeline.AnchoredPosition += new Vector2(0, -EnemyTimelineSize.y);
            alliedTimeline.Size = EnemyTimelineSize;
            return alliedTimeline;
        }

        private CharacterTimelineView GenerateTimeline()
        {
            CharacterTimelineView timeline = Instantiate(BattlePrefabsConfig.Instance.CharacterTimelinePrefab);
            timeline.GetTransform().SetParent(transform);
            timeline.GetTransform().localScale = Vector3.one;
            timeline.AnchoredPosition = new Vector2(0, timeline.Size.y / 2);

            return timeline;
        }

        private void CreateTimelineTimer(Line MovementLine)
        {
            TimerView = Instantiate(BattlePrefabsConfig.Instance.TimerPrefab);
            TimerView.GetTransform().SetParent(transform);
            TimerView.GetTransform().localScale = Vector3.one;
            TimerView.SetMovementLine(MovementLine);
            TimerView.SetBattleSystem(BattleSystemCached);
            TimerView.WorldPosition = MovementLine.StartPoint;
        }
    }
}