using System.Collections;
using System.Collections.Generic;
using TimelineHero.UI;
using TMPro;
using UnityEngine;

namespace TimelineHero.Battle
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