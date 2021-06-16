using TimelineHero.CoreUI;
using UnityEngine.SceneManagement;

namespace TimelineHero.MapUI
{
    public class EnemyWindow : Window
    {
        public void StartBattle()
        {
            SceneManager.LoadScene(1);
        }
    }
}