using DG.Tweening;
using System;
using System.Collections.Generic;
using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView
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
            EquipmentDeckCached.OnDiscardCard += DiscardCard;
            EquipmentDeckCached.OnDiscardAllCards += DiscardAllCards;
        }

        public void CreateCard(Skill SkillRef)
        {
            CardWrapper cardWrapper = MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().CardWrapperPrefab);
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

        private void DiscardCard(Skill SkillRef)
        {
            var card = Cards.Find(x => x.GetOriginalSkill() == SkillRef);
            if (card == null)
                return;

            card.transform.DOMove(new Vector3(-10, -5, 0), 1.0f).onComplete += card.DestroyUiObject;
        }

        private void DiscardAllCards()
        {
            var cardsToDiscard = new List<CardWrapper>(Cards);
            Cards.Clear();

            foreach (var card in cardsToDiscard)
            {
                card.transform.DOMove(new Vector3(-10, -5, 0), 1.0f).onComplete += card.DestroyUiObject;
            }
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

        //private void OnDestroy()
        //{
        //    if (EquipmentDeckCached != null)
        //        EquipmentDeckCached.OnDrawCard -= CreateCard;
        //}

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(new Vector3(WorldCenterPosition.x, WorldCenterPosition.y, 2000), new Vector3(0.1f, 0.1f, 0.2f));
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(new Vector3(WorldPosition.x, WorldPosition.y, 2000), new Vector3(0.1f, 0.1f, 0.2f));
        }
    }
}