using System.Collections.Generic;
using System.Linq;
using TimelineHero.Battle;
using TimelineHero.Character;
using TimelineHero.Core.Utils;

public static class ActionExecutionPool
{
    public static Dictionary<CharacterActionType, ActionExecutionBase> _ActionExecutionPool;

    public static void CreatePool()
    {
        _ActionExecutionPool = CoreUtils.GetEnumerableOfType<ActionExecutionBase>().ToDictionary(x => x.ActionType);
    }

    public static ActionExecutionBase GetActionExecution(CharacterActionType ActionType)
    {
        if (_ActionExecutionPool == null)
        {
            CreatePool();
        }

        if (!_ActionExecutionPool.ContainsKey(ActionType))
        {
            return null;
        }

        return _ActionExecutionPool[ActionType];
    }

    public static ActionExecutionBase GetActionExecution(ActionExecutionStage Stage, CharacterActionType ActionType)
    {
        var result = GetActionExecution(ActionType);

        if (result?.Stage != Stage)
            return null;

        return result;
    }
}
