using TMPro;
using UnityEngine;

namespace TimelineHero.BattleUI
{
    public class DiscardDeckButton : MonoBehaviour
    {
        public TextMeshProUGUI CardsCountText;

        public void SetValue(int Value)
        {
            CardsCountText.text = Value.ToString();
        }

        public void OnPressed()
        {

        }
    }
}