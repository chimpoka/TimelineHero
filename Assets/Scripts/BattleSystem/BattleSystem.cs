using System.Collections.Generic;
using TimelineHero.Core;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class BattleSystem
    {
        //public GameInstance Game;

        private BattleStateBase BattleState;
        public BattleTimelineTimer TimelineTimer;

        public List<CharacterBase> GetAlliedCharacters()
        {
            return GameInstance.Instance.GetAllies();
        }

        public List<CharacterBase> GetEnemyCharacters()
        {
            return GameInstance.Instance.GetEnemies();
        }
    }
}