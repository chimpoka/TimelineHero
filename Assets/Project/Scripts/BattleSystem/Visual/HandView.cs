using System.Collections.Generic;
using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class HandView : UiComponent, IHandView
    {
        public class EquipmentSetView : List<EquipmentBattleView>
        {
            public EquipmentSet EquipmentSetCached;

            public void UpdateOrder()
            {
                foreach (var slot in (int[])System.Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (this[slot]) this[slot].transform.SetSiblingIndex(slot);
                }
            }

            public void SetEquipmentInSlot(EquipmentBattleView NewEquipment, EquipmentSlot Slot)
            {
                var equipment = this[(int)Slot];
                if (equipment) equipment.DestroyUiObject();

                this[(int)Slot] = NewEquipment;

                if (Slot == EquipmentSlot.LeftHand || Slot == EquipmentSlot.RightHand)
                {
                    if (equipment = this[(int)EquipmentSlot.TwoHands]) equipment.DestroyUiObject();
                }
                else if (Slot == EquipmentSlot.TwoHands)
                {
                    if (equipment = this[(int)EquipmentSlot.RightHand]) equipment.DestroyUiObject();
                    if (equipment = this[(int)EquipmentSlot.LeftHand]) equipment.DestroyUiObject();
                }
            }

            public void Initialize()
            {
                EquipmentSetCached = CharacterPool.GetCurrentAlliedCharacter().CurrentEquipment;

                foreach (var slot in (int[])System.Enum.GetValues(typeof(EquipmentSlot)))
                {
                    Add(null);
                }
            }
        }

        public System.Action<CardWrapper> OnCardCreated;

        private EquipmentSetView CurrentEquipmentSet = new EquipmentSetView();

        public void Initialize()
        {
            BattleSystem.Get().OnBattleEquipmentCreationCommited += CreateEquipmentView;
            CurrentEquipmentSet.Initialize();
            BattleSystem.Get().CreateStartEquipment();
        }

        public void CreateEquipmentView(Equipment EquipmentData, EquipmentSlot Slot)
        {
            EquipmentBattleView NewEquipment = UnityEngine.MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().EquipmentBattlePrefab);
            NewEquipment.SetParent(transform);
            NewEquipment.Initialize(EquipmentData);
            CurrentEquipmentSet.SetEquipmentInSlot(NewEquipment, Slot);
            CurrentEquipmentSet.UpdateOrder();

            foreach (var deck in NewEquipment.Decks)
            {
                deck.OnCardCreated += OnCardCreated;
            }
        }

        public Transform GetParent()
        {
            return transform.parent;
        }

        public void AddCard(CardWrapper CardWrapperRef)
        {
            CardWrapperRef.EquipmentDeckCached.AddCard(CardWrapperRef);

        }

        public void RemoveCard(CardWrapper CardWrapperRef)
        {
            CardWrapperRef.EquipmentDeckCached.RemoveCard(CardWrapperRef);
        }
    }
}