using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Map
{
    public enum NodeType
    {
        Enemy, EliteEnemy, Town, Rest, Boss, Examination, Loot,
        Question, Relic, Sanctuary, Trap, StartNode
    }
    
    public class MapNode : MonoBehaviour
    {
        public List<MapNode> NeighbourNodes;

        public void AddNeighbour(MapNode NewNode)
        {
            if (NeighbourNodes.Contains(NewNode))
                return;
            
            NeighbourNodes.Add(NewNode);
        }

        public bool IsNeighbour(MapNode Node)
        {
            return NeighbourNodes.Contains(Node);
        }
    }
}