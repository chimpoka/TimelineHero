using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace TimelineHero.CoreUI
{
    public class UiComponent : MonoBehaviour
    {
        public Vector2 AnchoredPosition { get => GetTransform().anchoredPosition; set => GetTransform().anchoredPosition = value; }
        public Vector2 CenterPosition { get => GetCenterPosition(); }
        public Vector2 Size { get => GetTransform().sizeDelta; set => GetTransform().sizeDelta = value; }
        public Vector2 WorldPosition { get => new Vector2(GetTransform().position.x, GetTransform().position.y); set => GetTransform().position = new Vector3(value.x, value.y, 0); }
        public Bounds WorldBounds { get => CalculateBounds(); }
        public Vector2 WorldCenterPosition { get => WorldBounds.center; set => WorldPosition = value + (GetTransform().pivot - new Vector2(0.5f, 0.5f)) * WorldBounds.size; }


        private RectTransform transformCached;
        TweenerCore<Vector2, Vector2, VectorOptions> tween;

        public RectTransform GetTransform()
        {
            transformCached = transformCached ?? GetComponent<RectTransform>();
            return transformCached;
        }

        public void SetParent(Transform Parent)
        {
            GetTransform().SetParent(Parent);
            GetTransform().localScale = Vector3.one;
            GetTransform().anchorMin = Vector3.zero;
            GetTransform().anchorMax = Vector3.zero;
        }

        public void SetAnchorsToCenter()
        {
            GetTransform().anchorMin = new Vector2(0.5f, 0.5f);
            GetTransform().anchorMax = new Vector2(0.5f, 0.5f);
        }

        public Vector2 GetPositionInAtPivotPoint(Vector2 PivotPoint)
        {
            return AnchoredPosition + (GetTransform().pivot - PivotPoint) * Size;
        }

        public Vector2 GetCenterPosition()
        {
            return GetPositionInAtPivotPoint(new Vector2(0.5f, 0.5f));
        }

        public Vector2 GetCornerPosition()
        {
            return GetPositionInAtPivotPoint(new Vector2(0.0f, 0.0f));
        }

        public TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPos(Vector2 Position, float Duration = 1.0f)
        {
            DOStop();
            tween = GetTransform().DOAnchorPos(Position, Duration);
            return tween;
        }

        public void DOStop()
        {
            if (tween != null)
            {
                tween.Kill();
            }
        }

        public void DestroyUiObject()
        {
            DOStop();
            Destroy(gameObject);
        }

        private Bounds CalculateBounds()
        {
            Vector3[] corners = new Vector3[4];
            GetTransform().GetWorldCorners(corners);

            Vector3 center = new Vector3((corners[1].x + corners[2].x) / 2, (corners[0].y + corners[1].y) / 2, 0);
            Bounds bounds = new Bounds(center, new Vector3(Mathf.Abs(corners[1].x - corners[2].x), Mathf.Abs(corners[0].y - corners[1].y), 0.0f));

            return bounds;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = new Color(1, 0, 0, 0.5f);
        //    Gizmos.DrawCube(new Vector3(WorldBounds.center.x, WorldBounds.center.y, 2000), new Vector3(WorldBounds.size.x, WorldBounds.size.y, 0.2f));
        //}
    }
}