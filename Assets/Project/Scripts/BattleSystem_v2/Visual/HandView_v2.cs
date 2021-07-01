using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle_v2;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView_v2
{
    public class HandView_v2 : UiComponent, IHandView
    {
        public class EquipmentSetView
        {
            public EquipmentBattleView LeftHandEquipment;
            public EquipmentBattleView RightHandEquipment;
            public EquipmentBattleView TwoHandsEquipment;
            public EquipmentBattleView BodyEquipment;
            public EquipmentBattleView BootsEquipnemt;
            public EquipmentBattleView ConsumableEquipment;

            public void UpdateOrder()
            {
                if (LeftHandEquipment) LeftHandEquipment.transform.SetSiblingIndex(0);
                if (RightHandEquipment) RightHandEquipment.transform.SetSiblingIndex(1);
                if (TwoHandsEquipment) TwoHandsEquipment.transform.SetSiblingIndex(2);
                if (BodyEquipment) BodyEquipment.transform.SetSiblingIndex(3);
                if (BootsEquipnemt) BootsEquipnemt.transform.SetSiblingIndex(4);
                if (ConsumableEquipment) ConsumableEquipment.transform.SetSiblingIndex(5);
            }

            public EquipmentBattleView GetEquipmentInSlot(EquipmentSlot Slot)
            {
                switch (Slot)
                {
                    case EquipmentSlot.LeftHand: return LeftHandEquipment;
                    case EquipmentSlot.RightHand: return RightHandEquipment;
                    case EquipmentSlot.TwoHands: return TwoHandsEquipment;
                    case EquipmentSlot.Body: return BodyEquipment;
                    case EquipmentSlot.Boots: return BootsEquipnemt;
                    case EquipmentSlot.Consumable: return ConsumableEquipment;
                    default: return null;
                }
            }

            public void SetEquipmentInSlot(EquipmentBattleView NewEquipment, EquipmentSlot Slot)
            {
                var equip = GetEquipmentInSlot(Slot);
                if (equip) equip.DestroyUiObject();

                switch (Slot)
                {
                    case EquipmentSlot.LeftHand: 
                        LeftHandEquipment = NewEquipment;
                        if (TwoHandsEquipment) TwoHandsEquipment.DestroyUiObject();
                        break;
                    case EquipmentSlot.RightHand: 
                        RightHandEquipment = NewEquipment;
                        if (TwoHandsEquipment) TwoHandsEquipment.DestroyUiObject();
                        break;
                    case EquipmentSlot.TwoHands: 
                        TwoHandsEquipment = NewEquipment;
                        if (LeftHandEquipment) LeftHandEquipment.DestroyUiObject();
                        if (RightHandEquipment) RightHandEquipment.DestroyUiObject();
                        break;
                    case EquipmentSlot.Body: BodyEquipment = NewEquipment; break;
                    case EquipmentSlot.Boots: BootsEquipnemt = NewEquipment; break;
                    case EquipmentSlot.Consumable: ConsumableEquipment = NewEquipment; break;
                }
            }
        }

        public System.Action<CardWrapper> OnCardCreated;

        private EquipmentSetView CurrentEquipmentSet = new EquipmentSetView();
        private EquipmentSet EquipmentSetCached;

        public void Initialize()
        {
            BattleSystem_v2.Get().OnBattleEquipmentCreationCommited += CreateEquipmentView;
            EquipmentSetCached = CharacterPool.GetCurrentAlliedCharacter().CurrentEquipment;
        }

        public void CreateEquipmentView(Equipment EquipmentData, EquipmentSlot Slot)
        {
            EquipmentBattleView NewEquipment = UnityEngine.MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().EquipmentBattlePrefab);
            NewEquipment.SetParent(transform);
            NewEquipment.Initialize(EquipmentData.Clone());
            CurrentEquipmentSet.SetEquipmentInSlot(NewEquipment, Slot);
            CurrentEquipmentSet.UpdateOrder();

            foreach (var deck in NewEquipment.Decks)
            {
                deck.OnCardCreated += OnCardCreated;
                deck.EquipmentDeckCached?.Draw();
            }
        }

        //public void AddEquipment(EquipmentBattleView NewEquipment)
        //{
        //    Equipment.Add(NewEquipment);
        //    NewEquipment.transform.SetParent(transform);
        //}

        //public void AddEquipment(List<EquipmentBattleView> NewEquipment)
        //{
        //    foreach (EquipmentBattleView equipment in NewEquipment)
        //    {
        //        AddEquipment(equipment);
        //    }
        //}

        public Transform GetParent()
        {
            return transform.parent;
        }

        public void AddCard(CardWrapper CardWrapperRef)
        {
            ((CardWrapper_v2)CardWrapperRef).EquipmentDeckCached.AddCard(CardWrapperRef);

        }

        public void RemoveCard(CardWrapper CardWrapperRef)
        {
            ((CardWrapper_v2)CardWrapperRef).EquipmentDeckCached.RemoveCard(CardWrapperRef);
        }
    }
}