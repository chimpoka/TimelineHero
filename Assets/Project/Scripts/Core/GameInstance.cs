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
        public float CanvasScaleFactor;

        [SerializeField]
        private List<CharacterAsset> AlliedCharactersAssets;
        [SerializeField]
        private List<CharacterAsset> EnemyCharactersAssets;

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
            List<CharacterBase> characters = new List<CharacterBase>();
            foreach (CharacterAsset asset in Assets)
            {
                characters.Add(asset.ToCharacter());
            }

            return characters;
        }
    }
}