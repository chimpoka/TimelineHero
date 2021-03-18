﻿using System.Collections.Generic;
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

        public void CreateActionEffect(List<ActionEffectData> EffectData)
        {
            foreach(ActionEffectData data in EffectData)
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
            effect.SetAnchorsToCenter();
            effect.SetText(Text);
        }
    }
}