using System.Collections.Generic;
using TimelineHero.Battle;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.Hud;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.BattleUI
{
    public class BattleHud : HudBase
    {
        [SerializeField]
        private Button PlayBattleButton;
        [SerializeField]
        private Button PreviousEnemyButton;
        [SerializeField]
        private Button NextEnemyButton;
        [SerializeField]
        private ActionExecutionView ActionExecution;
        [SerializeField]
        private DrawDeckButton DrawDeckButtonCached;
        [SerializeField]
        private DiscardDeckButton DiscardDeckButtonCached;

        public System.Action OnPlayBattleButtonEvent;

        private Dictionary<CharacterBase, CharacterStatusView> CharactersStatuses = new Dictionary<CharacterBase, CharacterStatusView>();

        public void Initialize()
        {
            BattleSystem.Get().OnActionExecuted += ActionExecution.CreateActionEffect;

            BattleSystem.Get().PlayerDrawDeck.OnDeckSizeChanged += DrawDeckButtonCached.SetValue;
            BattleSystem.Get().PlayerDiscardDeck.OnDeckSizeChanged += DiscardDeckButtonCached.SetValue;

            BattleSystem.Get().BattleBoard.AlliedTimeline.OnLengthChanged += OnAlliedTimelineLenghtChanged;
        }

        public void SetPlayState()
        {
            PlayBattleButton.interactable = false;
            PreviousEnemyButton.interactable = false;
            NextEnemyButton.interactable = false;
        }

        public void SetConstructState()
        {
            PreviousEnemyButton.interactable = true;
            NextEnemyButton.interactable = true;
        }

        public void OnPlayBattleButton()
        {
            OnPlayBattleButtonEvent?.Invoke();
        }

        public void SlowDownGameSpeed()
        {
            Time.timeScale /= 2;
        }

        public void SpeedUpGameSpeed()
        {
            Time.timeScale *= 2;
        }

        public void CreatePreviousEnemy()
        {
            BattleSystem.Get().CreatePreviousEnemy();
        }

        public void CreateNextEnemy()
        {
            BattleSystem.Get().CreateNextEnemy();
        }

        public void UpdateStatuses(Vector2 AlliedStatusPosition, Vector2 EnemyStatusPosition)
        {
            foreach(var status in CharactersStatuses)
            {
                status.Value.DestroyUiObject();
            }
            CharactersStatuses = new Dictionary<CharacterBase, CharacterStatusView>();

            CreateStatuses(BattleSystem.Get().GetAlliedCharacters(), AlliedStatusPosition);
            CreateStatuses(new List<CharacterBase> { BattleSystem.Get().GetCurrentEnemy() }, EnemyStatusPosition);
        }

        private void CreateStatuses(List<CharacterBase> Characters, Vector2 Position)
        {
            foreach (CharacterBase character in Characters)
            {
                CharacterStatusView status = Instantiate(BattlePrefabsConfig.Instance.CharacterStatusPrefab);
                status.SetParent(transform);
                status.WorldPosition = Position + new Vector2(status.WorldBounds.extents.x, 0);
                Position += new Vector2(status.WorldBounds.size.x, 0);

                CharactersStatuses.Add(character, status);
                character.OnHealthChanged += SetHealth;
                character.OnAdrenalineChanged += SetAdrenaline;
                SetHealth(character);
                SetAdrenaline(character);
            }
        }

        private void SetHealth(CharacterBase Character)
        {
            CharactersStatuses[Character].SetHealth(Character.Health, Character.MaxHealth);
        }

        private void SetAdrenaline(CharacterBase Character)
        {
            CharactersStatuses[Character].SetAdrenaline(Character.Adrenaline);
        }

        private void OnAlliedTimelineLenghtChanged()
        {
            int length = BattleSystem.Get().BattleBoard.AlliedTimeline.Length;
            PlayBattleButton.interactable = length == 0 ? false : true ; 
        }
    }
}