using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/Equipment")]
    public class EquipmentAsset : ScriptableObject
    {
        [HideLabel]
        [PreviewField(Alignment = ObjectFieldAlignment.Left)]
        public Sprite EquipmentIcon;
        [HideLabel]
        public EquipmentType Type;
        public List<SkillSetAsset> SkillSets = new List<SkillSetAsset>();
    }
}