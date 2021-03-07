using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimelineHero.UI
{
    public class LoseWindow : UiComponent
    {
        public void OnPressButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}