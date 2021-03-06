﻿using DG.Tweening;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.BattleView
{
    public class TimelineStepView : UiComponent
    {
        [SerializeField]
        private TextMeshProUGUI ValueText;
        [HideInInspector]
        public bool DisabledInPlayState = false;
        [HideInInspector]
        public Action ActionCached;

        private Material ImageMaterial;

        private void Awake()
        {
            GetComponent<Image>().material = new Material(BattlePrefabsConfig.Get().DissolveMaterial);
            ImageMaterial = GetComponent<Image>().material;
        }

        public void SetValue(int Value)
        {
            if (ValueText != null)
            {
                ValueText.text = Value.ToString();
            }
        }

        public void PlayDestroyAnimation(float Duration)
        {
            ImageMaterial.DOFloat(0.0f, "_Fade", Duration).SetEase(Ease.Unset);
        }

        public void PlayRestoreAnimation(float Duration)
        {
            ImageMaterial.DOFloat(1.0f, "_Fade", Duration).SetEase(Ease.Unset);
        }
    }
}