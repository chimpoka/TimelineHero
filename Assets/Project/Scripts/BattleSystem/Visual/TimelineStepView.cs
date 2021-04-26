using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.Battle
{
    public class TimelineStepView : UiComponent
    {
        [SerializeField]
        private TextMeshProUGUI ValueText;
        [HideInInspector]
        public bool DisabledInPlayState = false;
        [HideInInspector]
        public Action ActionCached;

        public void SetValue(int Value)
        {
            if (ValueText != null)
            {
                ValueText.text = Value.ToString();
            }
        }

        public void PlayAnimation(float Duration)
        {
            GetComponent<Image>().material = new Material(BattlePrefabsConfig.Instance.DissolveMaterial);
            GetComponent<Image>().material.DOFloat(0.1f, "_Fade", Duration).SetEase(Ease.Unset);
        }
    }
}