using System.Collections;
using System.Collections.Generic;
using TimelineHero.Battle;
using TimelineHero.CoreUI;
using UnityEngine;


namespace TimelineHero.UI
{
    public class ActionExecutionView : UiComponent
    {
        public ActionEffect EffectPrefab;

        public RectTransform EnemyActionEmitter;
        public RectTransform AlliedActionEmitter;

        public void CreateActionEffect(ActionEffectData[] EffectData)
        {
            foreach(ActionEffectData data in EffectData)
            {
                if (!(data.AlliedText == null || data.AlliedText == ""))
                {
                    InstantiateActionEffect(AlliedActionEmitter, data.AlliedText);
                }
                if (!(data.EnemyText == null || data.EnemyText == ""))
                {
                    InstantiateActionEffect(EnemyActionEmitter, data.EnemyText);
                }
            }
        }

        private void InstantiateActionEffect(RectTransform Emitter, string Text)
        {
            ActionEffect effect = Instantiate(EffectPrefab);
            effect.SetParent(Emitter);
            effect.SetAnchorsToCenter();
            effect.SetText(Text);
        }
    }
}