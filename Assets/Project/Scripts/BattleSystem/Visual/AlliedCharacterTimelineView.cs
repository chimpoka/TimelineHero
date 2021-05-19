using System.Linq;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class AlliedCharacterTimelineView : CharacterTimelineView
    {
        private CardWrapper InvisibleCard;

        public bool TryInsertInvisibleCard(CardWrapper NewCard)
        {
            if (Cards.Count == 0)
            {
                return TryAddCard(InvisibleCard);
            }

            float newCardStartPosition = NewCard.WorldBounds.min.x;

            if (newCardStartPosition > Cards.Last().WorldBounds.center.x)
            {
                return TryAddCard(InvisibleCard);
            }

            float previousCardEndPosition = WorldBounds.min.x;
            float previousCardHandMinusPreBattleSize = 0;

            int i = 0;
            foreach (CardWrapper card in Cards)
            {
                if (card == InvisibleCard)
                {
                    previousCardEndPosition += previousCardHandMinusPreBattleSize;
                    continue;
                }

                float currentCardSize = card.WorldBounds.size.x;
                float currentCardHandSize = card.HandCard.WorldBounds.size.x;
                float currentCardStartPosition = previousCardEndPosition;
                float currentHandCardEndPosition = currentCardStartPosition + currentCardHandSize;
                float currentHandCardCenterPosition = currentCardStartPosition + currentCardHandSize / 2.0f;

                if (newCardStartPosition < currentHandCardEndPosition)
                {
                    int index = newCardStartPosition < currentHandCardCenterPosition ? i : i + 1;

                    Cards.Remove(InvisibleCard);
                    if (!TryInsertCard(InvisibleCard, index) && !TryAddCard(InvisibleCard))
                    {
                        UpdateCardsLayout(true);
                        return false;
                    }

                    return true;
                }

                previousCardEndPosition += currentCardSize;
                previousCardHandMinusPreBattleSize = currentCardHandSize - currentCardSize;

                i++;
            }

            return false;
        }

        public bool TryInsertVisibleCard(CardWrapper NewCard)
        {
            if (!Cards.Contains(InvisibleCard))
                return false;

            ReplaceWith(InvisibleCard, NewCard);
            return true;
        }

        public void CreateInvisibleCard(CardWrapper FromCard)
        {
            InvisibleCard = MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().CardWrapperPrefab);
            InvisibleCard.SetParent(GetTransform());
            InvisibleCard.SetState(CardState.Hand, FromCard.GetOriginalSkill());
            InvisibleCard.gameObject.SetActive(false);
        }

        public void DestroyInvisibleCard()
        {
            if (InvisibleCard == null)
                return;

            RemoveCard(InvisibleCard);
            InvisibleCard.DestroyUiObject();
        }

        public void RemoveInvisibleCard()
        {
            if (!Cards.Remove(InvisibleCard))
                return;
            
            UpdateCardsLayout(true);
        }
    }
}
