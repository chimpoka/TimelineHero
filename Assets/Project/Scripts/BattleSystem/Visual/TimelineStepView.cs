using System.Collections;
using System.Collections.Generic;
using TimelineHero.CoreUI;
using TMPro;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class TimelineStepView : UiComponent
    {
        [SerializeField]
        private TextMeshProUGUI ValueText;

        public void SetValue(int Value)
        {
            if (ValueText != null)
            {
                ValueText.text = Value.ToString();
            }
        }
    }
}