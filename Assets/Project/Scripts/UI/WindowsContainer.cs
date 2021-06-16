using System.Collections.Generic;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.CoreUI
{
    [CreateAssetMenu(menuName = "ScriptableObject/WindowsContainer")]
    public class WindowsContainer : SingletonScriptableObject<WindowsContainer>
    {
        public List<Window> WindowPrefabs;
    }
}