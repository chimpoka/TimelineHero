using TimelineHero.MapView;
using UnityEngine;

namespace TimelineHero.Map
{
    public class MapUtils
    {
        private static GameObject LinesParentObject;
        public static void DrawLine(Vector3 From, Vector3 To)
        {
            LineRenderer Line = MonoBehaviour.Instantiate(MapPrefabsConfig.Get().LinePrefab);
            Line.SetPosition(0, From);
            Line.SetPosition(1, (From + To) / 2f);
            Line.SetPosition(2, To);
            
            LinesParentObject = LinesParentObject ?? new GameObject("Lines");
            Line.transform.SetParent(LinesParentObject.transform);
        }
    }
}