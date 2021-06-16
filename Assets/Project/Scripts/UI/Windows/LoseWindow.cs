using TimelineHero.CoreUI;
using UnityEngine.SceneManagement;

namespace TimelineHero.BattleUI
{
    public class LoseWindow : Window
    {
        public void OnPressButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}