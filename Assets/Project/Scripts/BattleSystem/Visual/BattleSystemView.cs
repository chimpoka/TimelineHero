using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Core;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class BattleSystemView : MonoBehaviour
    {
        public Hand PlayerHand;
        public Board BattleBoard;
        public DrawDeck PlayerDrawDeck;
        public DiscardDeck PlayerDiscardDeck;
        public BattleController PlayerBattleController;

        public void Initialize()
        {
            GameInstance.Instance.CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            BattleBoard.Initialize();

            PlayerDrawDeck = new DrawDeck();
            List<Skill> AllAlliedSkills = new List<Skill>();
            foreach (CharacterBase character in BattleSystem.Get().GetAlliedCharacters())
            {
                AllAlliedSkills.AddRange(new List<Skill>(character.Skills));
            }
            PlayerDrawDeck.Add(AllAlliedSkills);

            PlayerDiscardDeck = new DiscardDeck();

            PlayerBattleController = new BattleController();
            PlayerBattleController.Initialize(PlayerHand, PlayerDrawDeck, PlayerDiscardDeck, BattleBoard);
        }

        public BattleTimelineTimerView GetTimerView()
        {
            return BattleBoard.GetTimerView();
        }

        public void SetActive(bool Active)
        {
            PlayerBattleController.SetActive(Active);
        }
    }
}