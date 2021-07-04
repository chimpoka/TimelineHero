using TimelineHero.CoreUI;
using UnityEngine;
using TMPro;

namespace TimelineHero.BattleUI
{
    public class CharacterStatusView : UiComponent
    {
        [SerializeField]
        private TextMeshProUGUI HealthText;
        [SerializeField]
        private TextMeshProUGUI ArmorText;
        [SerializeField]
        private TextMeshProUGUI AdrenalineText;
        [SerializeField]
        private GameObject AdrenalinePanel;

        public void SetHealth(int Health, int MaxHealth)
        {
            HealthText.text = Health.ToString() + " / " + MaxHealth.ToString();
        }

        public void SetArmor(int Armor)
        {
            ArmorText.text = Armor.ToString();
        }

        public void SetAdrenaline(int Adrenaline)
        {
            SetAdrenalinePanelVisibility(Adrenaline > 0);
            AdrenalineText.text = Adrenaline.ToString();
        }

        public void SetAdrenalinePanelVisibility(bool Visible)
        {
            AdrenalinePanel.SetActive(Visible);
        }
    }
}