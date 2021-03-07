using TimelineHero.CoreUI;
using UnityEngine;
using TMPro;

namespace TimelineHero.Battle
{
    public class CharacterStatusView : UiComponent
    {
        [SerializeField]
        private TextMeshProUGUI HealthText;
        [SerializeField]
        private TextMeshProUGUI ArmorText;

        public void SetHealth(int Health, int MaxHealth)
        {
            HealthText.text = Health.ToString() + " / " + MaxHealth.ToString();
        }

        public void SetArmor(int Armor)
        {
            ArmorText.text = Armor.ToString();
        }
    }
}