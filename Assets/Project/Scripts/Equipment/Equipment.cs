using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimelineHero.Character
{
    public enum EquipmentType { OneHand, TwoHands, Body, Boots, Consumable }
    public enum EquipmentSlot { LeftHand, RightHand, TwoHands, Body, Boots, Consumable }

    public class Equipment
    {
        public Equipment() { }

        public Equipment(Equipment other)
        {
            Name = other.Name;
            EquipmentIcon = other.EquipmentIcon;
            Type = other.Type;
            Slot = other.Slot;
            EquipmentDecks = other.EquipmentDecks;
        }

        public string Name;
        public Sprite EquipmentIcon;
        public EquipmentType Type;
        public EquipmentSlot Slot;
        public List<Battle.EquipmentDeck> EquipmentDecks;

        public Equipment Clone()
        {
            Equipment clone = new Equipment(this);
            clone.EquipmentDecks = EquipmentDecks.Select(deck => deck.Clone()).ToList();
            return clone;
        }

        public void DiscardAllCards()
        {
            EquipmentDecks.ForEach(x => x.DiscardAll());
        }

        public void DrawCardInEachSlot()
        {
            EquipmentDecks.ForEach(x => x.Draw());
        }
    }

    public class EquipmentSet : List<Equipment>
    {
        public void SetEquipmentInSlot(Equipment NewEquipment, EquipmentSlot Slot)
        {
            NewEquipment.Slot = Slot;
            this[(int)Slot] = NewEquipment;

            if (Slot == EquipmentSlot.LeftHand || Slot == EquipmentSlot.RightHand)
            {
                this[(int)EquipmentSlot.TwoHands] = null;
            }
            else if (Slot == EquipmentSlot.TwoHands)
            {
                this[(int)EquipmentSlot.RightHand] = null;
                this[(int)EquipmentSlot.LeftHand] = null;
            }
        }

        public void RefreshDeckAndOpenOneCard()
        {
            DiscardAllCards();
            DrawCardInEachSlot();
        }

        public void DiscardAllCards()
        {
            ForEach(x => x?.DiscardAllCards());
        }

        public void DrawCardInEachSlot()
        {
            ForEach(x => x?.DrawCardInEachSlot());
        }
    }
}