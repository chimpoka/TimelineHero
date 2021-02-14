using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    [CreateAssetMenu(menuName = "ScriptableObject/BattlePrefabs")]
    public class BattlePrefabsScriptableObject : SingletonScriptableObject<BattlePrefabsScriptableObject>
    {
        public SkillView SkillPrefab;
    }
}