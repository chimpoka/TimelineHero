using System.Collections;
using System.Collections.Generic;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.BattleView_v2
{
    public class EquipmentBattleView : UiComponent
    {
        public List<EquipmentDeckView> Decks;
        public List<RectTransform> SkillNames;
        public Image Icon;
        public RectTransform BodyTransform;

        public Equipment EquipmentCached;

        private int MaxFieldsCount;
        private float BorderOffset;

        private void Awake()
        {
            MaxFieldsCount = Decks.Count;
            BorderOffset = Mathf.Abs(BodyTransform.offsetMin.x) + Mathf.Abs(BodyTransform.offsetMax.x);
        }

        public void SetFieldsNumber(int Count)
        {
            if (Count <= 0 || Count > MaxFieldsCount)
                return;

            for (int i = 0; i < MaxFieldsCount; ++i)
            {
                Decks[i].gameObject.SetActive(i < Count);
                SkillNames[i].gameObject.SetActive(i < Count);

                SkillNames[i].anchorMin = new Vector2(i / (float)Count, 1);
                SkillNames[i].anchorMax = new Vector2((i + 1) / (float)Count, 1);
            }

            Size = new Vector2((Size.x - BorderOffset) * Count / MaxFieldsCount + BorderOffset, Size.y); 
        }

        public void SetIcon(Sprite NewIcon)
        {
            Icon.sprite = NewIcon;
        }

        public void Initialize(Equipment EquipmentRef)
        {
            EquipmentCached = EquipmentRef;
            SetIcon(EquipmentRef.EquipmentIcon);
            SetFieldsNumber(EquipmentRef.EquipmentDecks.Count);

            for (int i = 0; i < EquipmentCached.EquipmentDecks.Count; ++i)
            {
                Decks[i].Initialize(EquipmentCached.EquipmentDecks[i]);
                //EquipmentCached.EquipmentDecks[i].Draw();
            }
        }

        //public void DestroyEquipment()
        //{
        //    foreach (var deck in Decks)
        //    {
        //        deck.DestroyCards();
        //    }
        //}
    }
}