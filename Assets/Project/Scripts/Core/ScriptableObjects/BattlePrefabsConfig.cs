using Sirenix.OdinInspector;
using TimelineHero.BattleUI;
using TimelineHero.BattleView_v2;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.BattleView
{
    [CreateAssetMenu(menuName = "ScriptableObject/BattlePrefabs")]
    public class BattlePrefabsConfig : SingletonScriptableObject<BattlePrefabsConfig>
    {
        public CardWrapper CardWrapperPrefab;
        public Card CardPrefab;
        public CharacterTimelineView TimelinePrefab;
        public AlliedCharacterTimelineView AlliedTimelinePrefab;
        public BattleTimelineTimerView TimerPrefab;
        public CharacterStatusView CharacterStatusPrefab;
        public Material DissolveMaterial;

        [Title("Battle_v2")]
        public EquipmentBattleView EquipmentBattlePrefab;
        public CardWrapper_v2 CardWrapperPrefab_v2;
    }
}