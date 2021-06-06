using System.Collections.Generic;
using TimelineHero.Core;
using TimelineHero.Map;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.MapView
{
    [CreateAssetMenu(menuName = "ScriptableObject/MapPrefabs")]
    public class MapPrefabsConfig : SingletonScriptableObject<MapPrefabsConfig>
    {
        [System.Serializable]
        public class NodeTypeToNodeIcon
        {
            public NodeType Type;
            public Sprite Icon;
        }
        
        public List<NodeTypeToNodeIcon> MapNodeIcons;
        public LineRenderer LinePrefab;
    }
}