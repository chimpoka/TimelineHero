using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class TimelineStepView : MonoBehaviour
    {
        RectTransform TransformCached;
        private void Awake()
        {
            TransformCached = GetComponent<RectTransform>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public Vector2 GetSize()
        {
            TransformCached = TransformCached ?? GetComponent<RectTransform>();
            return new Vector2(TransformCached.rect.width, TransformCached.rect.height);
        }
    }
}