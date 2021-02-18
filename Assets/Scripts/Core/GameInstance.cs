﻿using System.Linq;
using UnityEngine;
using TimelineHero.Character;
using System.Collections.Generic;

namespace TimelineHero.Core 
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInstance")]
    public class GameInstance : ScriptableObject
    {
        static GameInstance _instance = null;
        public static GameInstance Instance
        {
            get
            {
                _instance = _instance ?? Resources.LoadAll<GameInstance>("").FirstOrDefault();
                return _instance;
            }
        }

        [SerializeField]
        private List<CharacterAsset> AlliedCharactersAssets;
        [SerializeField]
        private List<CharacterAsset> EnemyCharactersAssets;

        public float CanvasScaleFactor;

        private List<CharacterBase> AlliedCharacters;
        private List<CharacterBase> EnemyCharacters;

        public List<CharacterBase> GetAllies()
        {
            if (AlliedCharacters == null || AlliedCharacters.Count == 0)
            {
                AlliedCharacters = GetCharactersFromAssets(AlliedCharactersAssets);
            }
            return AlliedCharacters;
        }

        public List<CharacterBase> GetEnemies()
        {
            if (EnemyCharacters == null || EnemyCharacters.Count == 0)
            {
                EnemyCharacters = GetCharactersFromAssets(EnemyCharactersAssets);
            }
            return EnemyCharacters;
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