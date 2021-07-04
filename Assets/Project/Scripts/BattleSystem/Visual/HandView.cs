using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class HandView : UiComponent, IHandView
    {
        public class EquipmentSetView
        {
            public EquipmentSet EquipmentSetCached;

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

                //UpdateEquipmentData();
            }

            private void UpdateEquipmentData()
            {
                EquipmentSetCached.LeftHandEquipment = LeftHandEquipment?.EquipmentCached;
                EquipmentSetCached.RightHandEquipment = RightHandEquipment?.EquipmentCached;
                EquipmentSetCached.TwoHandsEquipment = TwoHandsEquipment?.EquipmentCached;
                EquipmentSetCached.BodyEquipment = BodyEquipment?.EquipmentCached;
                EquipmentSetCached.BootsEquipnemt = BootsEquipnemt?.EquipmentCached;
                EquipmentSetCached.ConsumableEquipment = ConsumableEquipment?.EquipmentCached;
            }
        }

        public System.Action<CardWrapper> OnCardCreated;

        private EquipmentSetView CurrentEquipmentSet = new EquipmentSetView();

        public void Initialize()
        {
            BattleSystem.Get().OnBattleEquipmentCreationCommited += CreateEquipmentView;
            CurrentEquipmentSet.EquipmentSetCached = CharacterPool.GetCurrentAlliedCharacter().CurrentEquipment;
            BattleSystem.Get().CreateStartEquipment();
        }

        public void CreateEquipmentView(Equipment EquipmentData, EquipmentSlot Slot)
        {
            if (CharacterPool.GetCurrentAlliedCharacter().CurrentEquipment.GetAllEquipment().Find(x => x == EquipmentData) != null)
            {
                print("Exist: " + EquipmentData.Name);
            }
            else
            {
                print("Not exist: " + EquipmentData.Name);
            }

            EquipmentBattleView NewEquipment = UnityEngine.MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().EquipmentBattlePrefab);
            NewEquipment.SetParent(transform);
            NewEquipment.Initialize(EquipmentData);
            CurrentEquipmentSet.SetEquipmentInSlot(NewEquipment, Slot);
            CurrentEquipmentSet.UpdateOrder();

            foreach (var deck in NewEquipment.Decks)
            {
                deck.OnCardCreated += OnCardCreated;
                //deck.EquipmentDeckCached?.Draw();
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