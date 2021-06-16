using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.CoreUI
{
    [CreateAssetMenu(menuName = "ScriptableObject/CorePrefabs")]
    public class CorePrefabsConfig : SingletonScriptableObject<CorePrefabsConfig>
    {
        public BackgroundWidget BackgroundWidgetPrefab;
    }
}