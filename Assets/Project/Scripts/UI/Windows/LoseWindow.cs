using TimelineHero.CoreUI;
using UnityEngine.SceneManagement;

namespace TimelineHero.BattleUI
{
    public class LoseWindow : UiComponent
    {
        public void OnPressButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}