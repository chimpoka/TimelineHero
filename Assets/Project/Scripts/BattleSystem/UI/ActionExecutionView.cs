using System.Collections.Generic;
using TimelineHero.Battle;
using TimelineHero.CoreUI;
using UnityEngine;


namespace TimelineHero.BattleUI
{
    public class ActionExecutionView : UiComponent
    {
        public ActionEffect EffectPrefab;

        public RectTransform EnemyActionEmitter;
        public RectTransform AlliedActionEmitter;

        public void CreateActionEffect(ActionData Data)
        {
            foreach(ActionEffectData data in Data.EffectData)
            {
                if (data == null)
                {
                    continue;
                }
                if (data.AlliedText != "")
                {
                    InstantiateActionEffect(AlliedActionEmitter, data.AlliedText);
                }
                if (data.EnemyText != "")
                {
                    InstantiateActionEffect(EnemyActionEmitter, data.EnemyText);
                }
            }
        }

        private void InstantiateActionEffect(RectTransform Emitter, string Text)
        {
            ActionEffect effect = Instantiate(EffectPrefab);
            effect.SetParent(Emitter);
            effect.StretchAnchors();
            effect.SetText(Text);
        }
    }
}