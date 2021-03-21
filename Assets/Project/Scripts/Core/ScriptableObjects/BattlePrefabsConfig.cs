using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    [CreateAssetMenu(menuName = "ScriptableObject/BattlePrefabs")]
    public class BattlePrefabsConfig : SingletonScriptableObject<BattlePrefabsConfig>
    {
        public CardWrapper CardWrapperPrefab;
        public Card CardPrefab;
        public CharacterTimelineView CharacterTimelinePrefab;
        public BattleTimelineTimerView TimerPrefab;
        public CharacterStatusView CharacterStatusPrefab;
    }
}