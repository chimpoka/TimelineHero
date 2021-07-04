using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/SkillSet")]
    public class SkillSetAsset : ScriptableObject
    {
        [GUIColor(1.0f, 1.0f, 0.0f, 1.0f)]
        public string SkillName;
        [ListDrawerSettings(ShowPaging = false)]
        public List<SkillAsset> Skills = new List<SkillAsset>();
    }
}