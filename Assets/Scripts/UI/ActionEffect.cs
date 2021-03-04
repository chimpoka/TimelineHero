using System.Collections;
using System.Collections.Generic;
using TimelineHero.CoreUI;
using TMPro;
using UnityEngine;

namespace TimelineHero.UI
{
    public class ActionEffect : UiComponent
    {
        public TextMeshProUGUI EffectText;

        public void SetText(string Text)
        {
            EffectText.text = Text;
        }

        public void Destroy()
        {
            MonoBehaviour.Destroy(gameObject);
        }
    }
}