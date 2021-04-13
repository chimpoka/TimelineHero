using System.Linq;
using UnityEngine;
using TimelineHero.Character;
using System.Collections.Generic;

namespace TimelineHero.Core 
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInstance")]
    public class GameInstance : SingletonScriptableObject<GameInstance>
    {
        public int DrawCardCount = 5;
        public float DelayBetweenCardAnimationsInSeconds = 0.3f;
        public bool ShuffleDrawDeck = true;
        [HideInInspector]
        public float CanvasScaleFactor;

        [SerializeField]
        private List<CharacterAsset> AlliedCharactersAssets;
        [SerializeField]
        private List<CharacterAsset> EnemyCharactersAssets;

        private Dictionary<string, CharacterBase> CharactersPool = new Dictionary<string, CharacterBase>();

        public CharacterBase GetCharacter(string Name)
        {
            if (!CharactersPool.ContainsKey(Name))
            {
                CharacterAsset asset = AlliedCharactersAssets.Find(x => x.name == Name);
                asset = asset ?? EnemyCharactersAssets.Find(x => x.name == Name);

                if (asset == null)
                {
                    Debug.LogError("Character asset '" + Name + "' doesn't exist!");
                }

                CharactersPool[Name] = asset.ToCharacter();
            }

            return CharactersPool[Name];
        }

        public Skill GetSkill(string ChatacterName, string SkillName)
        {
            return GetCharacter(ChatacterName).GetSkill(SkillName);
        }

        public List<CharacterBase> GetAllies()
        {
            return GetCharactersFromAssets(AlliedCharactersAssets);
        }

        public List<CharacterBase> GetEnemies()
        {
            return GetCharactersFromAssets(EnemyCharactersAssets);
        }

        private List<CharacterBase> GetCharactersFromAssets(List<CharacterAsset> Assets)
        {
            return Assets.Select(asset => asset.ToCharacter()).ToList();
        }
    }
}