using System.Collections.Generic;
using TimelineHero.Battle_v2;
using TimelineHero.BattleUI;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TimelineHero.BattleUI_v2
{
    public class BattleHud_v2 : Hud<BattleHud_v2>
    {
        [SerializeField] private GameObject DebugButtonsContainer;
        [SerializeField] private Button PlayBattleButton;
        [SerializeField] private ActionExecutionView ActionExecution;

        public System.Action OnPlayBattleButtonEvent;

        private Dictionary<CharacterBase, CharacterStatusView> CharactersStatuses =
            new Dictionary<CharacterBase, CharacterStatusView>();

        public void Initialize()
        {
            BattleSystem_v2.Get().OnActionExecuted += ActionExecution.CreateActionEffect;
            BattleSystem_v2.Get().BattleBoard.AlliedTimeline.OnLengthChanged += OnAlliedTimelineLenghtChanged;
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
            BattleSystem_v2.Get().CreateNextEnemySkillSet();
        }

        public void CreatePreviousEnemySkillSet()
        {
            BattleSystem_v2.Get().CreatePreviousEnemySkillSet();
        }

        public void CreatePreviousEnemy()
        {
            BattleSystem_v2.Get().CreatePreviousEnemy();
        }

        public void CreateNextEnemy()
        {
            BattleSystem_v2.Get().CreateNextEnemy();
        }

        public void UpdateStatuses(Vector2 AlliedStatusPosition, Vector2 EnemyStatusPosition)
        {
            foreach (var status in CharactersStatuses)
            {
                status.Value.DestroyUiObject();
            }
            CharactersStatuses = new Dictionary<CharacterBase, CharacterStatusView>();

            CreateStatuses(CharacterPool.GetAlliedCharacters(), AlliedStatusPosition);
            CreateStatuses(new List<CharacterBase> {BattleSystem_v2.Get().GetCurrentEnemy()}, EnemyStatusPosition);
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
            int length = BattleSystem_v2.Get().BattleBoard.AlliedTimeline.Length;
            PlayBattleButton.interactable = length != 0;
        }
    }
}