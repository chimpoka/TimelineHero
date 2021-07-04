using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/Character/EnemyCharacter")]
    public class EnemyCharacterAsset : CharacterAssetBase
    {
        public List<SkillSetAsset> SkillSets = new List<SkillSetAsset>();

        public EnemyCharacter ToCharacter()
        {
            EnemyCharacter character = new EnemyCharacter();
            SetBaseData(character);
            character.SkillSets = SkillSets.Select(skillSetAsset => ConvertSkillSetAsset(skillSetAsset, character)).ToList();

            return character;
        }
    }
}