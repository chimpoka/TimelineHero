using TimelineHero.CoreUI;
using TMPro;
using UnityEngine;

namespace TimelineHero.BattleUI
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