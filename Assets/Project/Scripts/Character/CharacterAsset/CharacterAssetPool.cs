using System.Collections.Generic;
using System.Linq;
using TimelineHero.Core;
using TimelineHero.Core.Utils;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/CharacterAssetPool")]
    public class CharacterAssetPool : SingletonScriptableObject<CharacterAssetPool>
    {
        [SerializeField] private List<CharacterAssetBase> AlliedCharacters;
        [SerializeField] private List<CharacterAssetBase> EnemyCharacters;

        public List<CharacterAssetBase> GetAlliedCharacters()
        {
            return AlliedCharacters;
        }

        public List<CharacterAssetBase> GetEnemyCharacters()
        {
            return EnemyCharacters;
        }
    }

    public static class CharacterPool
    {
        private static List<CharacterBase> _AlliedCharacterPool;
        private static List<CharacterBase> _EnemyCharacterPool;

        public static List<CharacterBase> GetAlliedCharacters()
        {
            if (_AlliedCharacterPool == null)
            {
                List<CharacterAssetBase> assets = CharacterAssetPool.Get().GetAlliedCharacters();
                _AlliedCharacterPool = assets.Select(asset => asset.ToCharacter()).ToList();
            }

            return _AlliedCharacterPool;
        }

        public static List<CharacterBase> GetEnemyCharacters()
        {
            if (_EnemyCharacterPool == null)
            {
                List<CharacterAssetBase> assets = CharacterAssetPool.Get().GetEnemyCharacters();
                _EnemyCharacterPool = assets.Select(asset => asset.ToCharacter()).ToList();
            }

            return _EnemyCharacterPool;
        }

        public static CharacterBase GetCurrentAlliedCharacter()
        {
            return GetAlliedCharacters()[0];
        }

        public static void ResetEnemyCharacters()
        {
            _EnemyCharacterPool?.Clear();
            _EnemyCharacterPool = null;
        }

        public static void ResetAlliedCharacters()
        {
            _AlliedCharacterPool?.Clear();
            _AlliedCharacterPool = null;
        }
    }
}