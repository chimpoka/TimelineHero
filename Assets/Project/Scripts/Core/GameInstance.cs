using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Core
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInstance")]
    public class GameInstance : SingletonScriptableObject<GameInstance>
    {
        [Title("Battle")]
        public int DrawCardCount = 5;
        public float DelayBetweenCardAnimationsInSeconds = 0.3f;
        public bool ShuffleDrawDeck = true;
        public float MinTimelineSpeed = 0.5f;
        public float MaxTimelineSpeed = 5.0f;
        public float FullAdrenalineAttackMultiplier = 1.0f;

        [Title("Map")]
        public float EnterNodeDelay = 0f;

        [HideInInspector] public float CanvasScaleFactor;
    }
}