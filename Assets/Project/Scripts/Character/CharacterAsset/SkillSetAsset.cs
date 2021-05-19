using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/SkillSet")]
    public class SkillSetAsset : ScriptableObject
    {
        [ListDrawerSettings(ShowPaging = false)]
        public List<SkillAsset> Skills = new List<SkillAsset>();
    }
}