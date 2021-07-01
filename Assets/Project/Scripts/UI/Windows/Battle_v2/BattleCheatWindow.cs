using System.Collections;
using System.Collections.Generic;
using TimelineHero.Battle_v2;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.BattleUI_v2
{
    public class BattleCheatWindow : Window
    {
        public int HeaderHeight = 60;

        public RectTransform HeadersLayout;
        public RectTransform EquipmentLayout;
        public RectTransform EquipmentGroupPrefab;

        protected override void StartOpenWindowEvent()
        {
            foreach (var slot in (EquipmentSlot[])System.Enum.GetValues(typeof(EquipmentSlot)))
            {
                TextMeshProUGUI header = new GameObject(slot.ToString()).AddComponent<TextMeshProUGUI>();
                header.transform.SetParent(HeadersLayout);
                header.transform.localScale = Vector3.one;
                header.alignment = TextAlignmentOptions.Center;
                header.text = slot.ToString();

                RectTransform equipmentGroup = Instantiate(EquipmentGroupPrefab);
                equipmentGroup.SetParent(EquipmentLayout);
                equipmentGroup.transform.localScale = Vector3.one;

                foreach (var equipment in EquipmentPool.GetAllEquipmentOfType(BattleUtils.ConvertEquipmentSlotToType(slot)))
                {
                    Button button = new GameObject(equipment.Name).AddComponent<Button>();
                    button.image = button.gameObject.AddComponent<Image>();
                    button.transform.SetParent(equipmentGroup);
                    button.transform.localScale = Vector3.one;
                    button.image.sprite = equipment.EquipmentIcon;
                    button.onClick.AddListener(() => BattleSystem_v2.Get().CreateBattleEquipment(equipment, slot));
                }
            }
        }
    }
}