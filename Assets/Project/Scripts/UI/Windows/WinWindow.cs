using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimelineHero.UI
{
    public class WinWindow : UiComponent
    {
        public void OnPressButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}