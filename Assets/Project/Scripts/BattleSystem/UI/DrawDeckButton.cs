using TMPro;
using UnityEngine;

namespace TimelineHero.BattleUI
{
    public class DrawDeckButton : MonoBehaviour
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