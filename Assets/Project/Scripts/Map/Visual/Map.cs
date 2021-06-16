using System;
using System.Collections.Generic;
using DG.Tweening;
using TimelineHero.Core;
using TimelineHero.Map;
using TimelineHero.MapUI;
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
                Player.SendPlayerToNode(SelectedNode).onComplete += OnNodeReached;
                CurrentNode = SelectedNode;
                InputSystem.Get().bWorldInputEnabled = false;
            }
            else
            {
                print("Node " + SelectedNode.name + " is not reachable");
            }
        }

        private void OnNodeReached()
        {
            InputSystem.Get().bWorldInputEnabled = true;
            DOTween.Sequence().AppendInterval(GameInstance.Get().EnterNodeDelay).OnComplete(() => EnterNode(CurrentNode));
        }

        private void EnterNode(MapNodeVisual Node)
        {
            switch (Node.Type)
            {
                case NodeType.Enemy:
                    MapHud.Get().OpenWindow<EnemyWindow>();
                    break;
                case NodeType.EliteEnemy:
                    MapHud.Get().OpenWindow<EnemyWindow>();
                    break;
                case NodeType.Town: 
                    MapHud.Get().OpenWindow<TownWindow>();
                    break;
                case NodeType.Rest:
                    MapHud.Get().OpenWindow<RestWindow>();
                    break;
                case NodeType.Boss:
                    MapHud.Get().OpenWindow<EnemyWindow>();
                    break;
                case NodeType.Examination:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                case NodeType.Loot:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                case NodeType.Question:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                case NodeType.Relic:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                case NodeType.Sanctuary:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                case NodeType.Trap:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                case NodeType.StartNode:
                    MapHud.Get().OpenWindow<TemplateMapWindow>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}