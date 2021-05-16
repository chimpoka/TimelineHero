using UnityEngine;

namespace TimelineHero.BattleUI
{
    public class BattleUiElementsOrderController : MonoBehaviour
    {
        [SerializeField] RectTransform Hand;
        [SerializeField] RectTransform Board;
        [SerializeField] RectTransform DrawDectButton;
        [SerializeField] RectTransform DiscardDeckButton;
        [SerializeField] RectTransform DiscardSection;

        private RectTransform Parent;

        private void Start()
        {
            Parent = (RectTransform)Hand.parent;
        }

        private void MoveToBackground(RectTransform Element)
        {
            Element.SetSiblingIndex(0);
        }

        private void MoveToForeground(RectTransform Element)
        {
            Element.SetSiblingIndex(Parent.childCount - 1);
        }

        public void SetDiscardCardLayout()
        {
            MoveToBackground(Board);

            MoveToForeground(Hand);
            MoveToForeground(DrawDectButton);
            MoveToForeground(DiscardDeckButton);
            MoveToForeground(DiscardSection);
            DiscardSection.gameObject.SetActive(true);
        }

        public void SetOverlookLayout()
        {
            MoveToBackground(Hand);

            MoveToForeground(Board);
            MoveToForeground(DrawDectButton);
            MoveToForeground(DiscardDeckButton);
            DiscardSection.gameObject.SetActive(false);
        }
    }
}