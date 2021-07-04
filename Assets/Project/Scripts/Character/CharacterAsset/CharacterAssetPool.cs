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
        [SerializeField] private List<AlliedCharacterAsset> AlliedCharacters;
        [SerializeField] private List<EnemyCharacterAsset> EnemyCharacters;

        public List<AlliedCharacterAsset> GetAlliedCharacters()
        {
            return AlliedCharacters;
        }

        public List<EnemyCharacterAsset> GetEnemyCharacters()
        {
            return EnemyCharacters;
        }
    }

    public static class CharacterPool
    {
        private static List<AlliedCharacter> _AlliedCharacterPool;
        private static List<EnemyCharacter> _EnemyCharacterPool;

        public static List<AlliedCharacter> GetAlliedCharacters()
        {
            if (_AlliedCharacterPool == null)
            {
                var assets = CharacterAssetPool.Get().GetAlliedCharacters();
                _AlliedCharacterPool = assets.Select(asset => asset.ToCharacter()).ToList();
            }

            return _AlliedCharacterPool;
        }

        public static List<EnemyCharacter> GetEnemyCharacters()
        {
            if (_EnemyCharacterPool == null)
            {
                var assets = CharacterAssetPool.Get().GetEnemyCharacters();
                _EnemyCharacterPool = assets.Select(asset => asset.ToCharacter()).ToList();
            }

            return _EnemyCharacterPool;
        }

        public static AlliedCharacter GetCurrentAlliedCharacter()
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