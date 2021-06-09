using System.Collections.Generic;
using TimelineHero.Map;
using UnityEngine;

namespace TimelineHero.MapView
{
    public class Map : MonoBehaviour
    {
        private MapPlayerTracker Player;
        private MapNodeVisual CurrentNode;
        private List<MapNodeVisual> NodesList = new List<MapNodeVisual>();

        private void Awake()
        {
            NodesList.AddRange(GetComponentsInChildren<MapNodeVisual>());
            CurrentNode = NodesList.Find(node => node.Type == NodeType.StartNode);
            Player = Instantiate(MapPrefabsConfig.Get().MapPlayerPrefab);
            Player.transform.position = CurrentNode.transform.position;

            foreach (var node in NodesList)
            {
                node.OnNodePressed += TrySendPlayerToNode;
            }
        }

        private void TrySendPlayerToNode(MapNodeVisual SelectedNode)
        {
            if (MapUtils.AreNeighbourNodes(CurrentNode, SelectedNode))
            {
                Player.SendPlayerToNode(SelectedNode);
                CurrentNode = SelectedNode;
            }
            else
            {
                print("Node " + SelectedNode.name + " is not reachable");
            }
        }
    }
}