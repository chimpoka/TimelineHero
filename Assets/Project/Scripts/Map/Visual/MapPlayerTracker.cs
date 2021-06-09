using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TimelineHero.MapView
{
    public class MapPlayerTracker : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer PlayerSprite;

        public TweenerCore<Vector3, Vector3, VectorOptions> SendPlayerToNode(MapNodeVisual Node)
        {
            return transform.DOMove(Node.transform.position, 0.7f);
        }
    }
}