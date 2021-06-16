using TimelineHero.CoreUI;
using UnityEngine.SceneManagement;

namespace TimelineHero.BattleUI
{
    public class WinWindow : Window
    {
        public void OnPressButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}