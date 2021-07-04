using System.Collections.Generic;
using TimelineHero.Battle;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TimelineHero.BattleUI
{
    public class BattleHud : Hud<BattleHud>
    {
        [SerializeField] private GameObject DebugButtonsContainer;
        [SerializeField] private Button PlayBattleButton;
        [SerializeField] private ActionExecutionView ActionExecution;

        public System.Action OnPlayBattleButtonEvent;

        private Dictionary<CharacterBase, CharacterStatusView> CharactersStatuses =
            new Dictionary<CharacterBase, CharacterStatusView>();

        public void Initialize()
        {
            BattleSystem.Get().OnActionExecuted += ActionExecution.CreateActionEffect;
            BattleSystem.Get().BattleBoard.AlliedTimeline.OnLengthChanged += OnAlliedTimelineLenghtChanged;
        }

        public void SetPlayBattleState()
        {
            PlayBattleButton.interactable = false;
            SetEnabledDebugButtons(false);
        }

        public void SetInitialBattleState()
        {
            PlayBattleButton.interactable = false;
            SetEnabledDebugButtons(true);
        }

        private void SetEnabledDebugButtons(bool Enabled)
        {
            foreach (Button button in DebugButtonsContainer.GetComponentsInChildren<Button>())
            {
                button.interactable = Enabled;
            }
        }

        public void OnGoToMapButton()
        {
            SceneManager.LoadScene(0);
        }

        public void OnCheatButton()
        {
            OpenWindow<BattleCheatWindow>();
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

        public void CreateNextEnemySkillSet()
        {
            BattleSystem.Get().CreateNextEnemySkillSet();
        }

        public void CreatePreviousEnemySkillSet()
        {
            BattleSystem.Get().CreatePreviousEnemySkillSet();
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
            foreach (var status in CharactersStatuses)
            {
                status.Key.OnHealthChanged -= SetHealth;
                status.Key.OnAdrenalineChanged -= SetAdrenaline;
                status.Value.DestroyUiObject();
            }
            CharactersStatuses = new Dictionary<CharacterBase, CharacterStatusView>();

            CreateStatuses(CharacterPool.GetAlliedCharacters(), AlliedStatusPosition);
            CreateStatuses(new List<CharacterBase> {BattleSystem.Get().GetCurrentEnemy()}, EnemyStatusPosition);
        }

        private void CreateStatuses(List<CharacterBase> Characters, Vector2 Position)
        {
            foreach (CharacterBase character in Characters)
            {
                CharacterStatusView status = Instantiate(BattlePrefabsConfig.Get().CharacterStatusPrefab);
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
            PlayBattleButton.interactable = length != 0;
        }

        private void OnDestroy()
        {
            foreach (var status in CharactersStatuses)
            {
                status.Key.OnHealthChanged -= SetHealth;
                status.Key.OnAdrenalineChanged -= SetAdrenaline;
            }
        }
    }
}