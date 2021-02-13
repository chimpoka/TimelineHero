using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    public class CharacterBase : MonoBehaviour
    {
        public CharacterBase(List<Skill> Skills)
        {
            this.Skills = Skills;
        }

        public List<Skill> Skills;
    }
}