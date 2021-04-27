using System.Collections.Generic;
using TimelineHero.Character;
using TimelineHero.Hud;
using TimelineHero.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.Battle
{
    public class BattleHud : HudBase
    {
        [SerializeField]
        private Button PlayBattleButton;
        [SerializeField]
        private ActionExecutionView ActionExecution;
        [SerializeField]
        private DrawDeckButton DrawDeckButtonCached;
        [SerializeField]
        private DiscardDeckButton DiscardDeckButtonCached;

        public System.Action OnPlayBattleButtonEvent;

        Dictionary<CharacterBase, CharacterStatusView> CharactersStatuses;

        private BattleSceneController BattleSceneControllerCached;

        public void SetBattleSceneController(BattleSceneController BattleSceneControllerRef)
        {
            BattleSceneControllerCached = BattleSceneControllerRef;

            Initialize();
        }

        private void Initialize()
        {
            CharactersStatuses = new Dictionary<CharacterBase, CharacterStatusView>();
            CreateAlliedCharacterStatuses();
            CreateEnemiesCharacterStatuses();
            BattleSceneControllerCached.Battle.OnActionExecuted += ActionExecution.CreateActionEffect;

            BattleSceneControllerCached.BattleView.PlayerDrawDeck.OnDeckSizeChanged += DrawDeckButtonCached.SetValue;
            BattleSceneControllerCached.BattleView.PlayerDiscardDeck.OnDeckSizeChanged += DiscardDeckButtonCached.SetValue;

            BattleSceneControllerCached.BattleView.BattleBoard.GetAlliedTimeline().OnLengthChanged += OnAlliedTimelineLenghtChanged;
        }

        public void SetPlayState()
        {
            PlayBattleButton.interactable = false;
        }

        public void SetConstructState()
        {
            PlayBattleButton.interactable = true;
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

        private void CreateAlliedCharacterStatuses()
        {
            Bounds timelineBounds = BattleSceneControllerCached.BattleView.BattleBoard.GetAlliedTimeline().WorldBounds;
            Vector2 statusPosition = new Vector2(timelineBounds.max.x, timelineBounds.center.y);
            CreateCharacterStatuses(BattleSceneControllerCached.Battle.GetAlliedCharacters(), statusPosition);
        }

        private void CreateEnemiesCharacterStatuses()
        {
            Bounds timelineBounds = BattleSceneControllerCached.BattleView.BattleBoard.GetEnemyTimeline().WorldBounds;
            Vector2 statusPosition = new Vector2(timelineBounds.max.x, timelineBounds.center.y);
            CreateCharacterStatuses(BattleSceneControllerCached.Battle.GetEnemyCharacters(), statusPosition);
        }

        private void CreateCharacterStatuses(List<CharacterBase> Characters, Vector2 Position)
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
            int length = BattleSceneControllerCached.BattleView.BattleBoard.GetAlliedTimeline().Length;
            PlayBattleButton.interactable = length == 0 ? false : true ; 
        }
    }
}