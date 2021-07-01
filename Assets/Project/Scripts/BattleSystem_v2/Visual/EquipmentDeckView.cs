using System.Collections;
using System.Collections.Generic;
using TimelineHero.Battle_v2;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView_v2
{
    public class EquipmentDeckView : UiComponent
    {
        public RectTransform CardLayout;

        private List<CardWrapper> Cards = new List<CardWrapper>();
        public EquipmentDeck EquipmentDeckCached;

        public System.Action<CardWrapper> OnCardCreated;

        public void Initialize(EquipmentDeck EquipmentDeckRef)
        {
            EquipmentDeckCached = EquipmentDeckRef;
            EquipmentDeckCached.OnDrawCard += CreateCard;
        }

        public void CreateCard(Skill SkillRef)
        {
            CardWrapper_v2 cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().CardWrapperPrefab_v2);
            cardWrapper.SetParent(CardLayout);
            cardWrapper.SetState(CardState.Hand, SkillRef);
            cardWrapper.SetToCenterOfParent();
            cardWrapper.EquipmentDeckCached = this;

            // TODO: Rework
            var offset = Cards.Count * new Vector2(20, 25);
            //var randOffset = new Vector2(Random.Range(-45, 45), Random.Range(-45, 45));
            cardWrapper.DOAnchorPos(cardWrapper.AnchoredPosition + offset);

            Cards.Add(cardWrapper);
            OnCardCreated?.Invoke(cardWrapper);
        }

        public void AddCard(CardWrapper NewCard)
        {
            Cards.Add(NewCard);
            NewCard.SetState(CardState.Hand, NewCard.GetOriginalSkill());
            NewCard.SetParent(CardLayout);
            ShrinkCards();
        }

        public void RemoveCard(CardWrapper CardToRemove)
        {
            Cards.Remove(CardToRemove);
            ShrinkCards();
        }

        public void OnPlusButton()
        {
            EquipmentDeckCached.Draw();
        }

        private void ShrinkCards()
        {
            for (int i = 0; i < Cards.Count; ++i)
            {
                var offset = i * new Vector2(20, 25);
                Cards[i].DOAnchorPos(Cards[i].GetCenterOfParent() + offset);
            }
        }

        private void OnDestroy()
        {
            if (EquipmentDeckCached != null)
                EquipmentDeckCached.OnDrawCard -= CreateCard;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(new Vector3(WorldCenterPosition.x, WorldCenterPosition.y, 2000), new Vector3(0.1f, 0.1f, 0.2f));
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(new Vector3(WorldPosition.x, WorldPosition.y, 2000), new Vector3(0.1f, 0.1f, 0.2f));
        }
    }
}