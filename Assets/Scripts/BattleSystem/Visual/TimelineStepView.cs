using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class TimelineStepView : MonoBehaviour
    {
        public SpriteRenderer StepSpriteRenderer;
        float PixelToUnit = 100;
        Vector4 Border = new Vector4(7, 7, 7, 7);
        Vector2 SpriteSize = new Vector2(0.2f, 0.8f);

        private void Awake()
        {
            StepSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void Create()
        {
            Texture2D stepTexture = Resources.Load<Texture2D>("Textures/TimelineStep");
            Sprite sprite = UnityEngine.Sprite.Create(stepTexture, new Rect(0, 0, stepTexture.width, stepTexture.height),
                new Vector2(0.0f, 0.0f), PixelToUnit, 0, SpriteMeshType.Tight, Border);

            SetSize(SpriteSize);
        }

        public void SetSprite(Sprite NewSprite)
        {
            StepSpriteRenderer.sprite = NewSprite;
        }

        public void SetSize(Vector2 Size)
        {
            SpriteSize = Size;
            StepSpriteRenderer.size = Size;
        }

        public Vector2 GetSize()
        {
            return SpriteSize;
        }
    }
}