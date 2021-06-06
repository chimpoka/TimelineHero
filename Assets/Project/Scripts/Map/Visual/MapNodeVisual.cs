using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TimelineHero.Map;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TimelineHero.MapView
{
    [SelectionBase]
    [ExecuteInEditMode]
    public class MapNodeVisual : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private TextMeshProUGUI NodeNameText;
        [SerializeField] private Canvas NodeNameCanvas;
        [SerializeField] private SpriteRenderer NodeImage;
        
        [Title("Settings")]
        public NodeType Type = NodeType.Enemy;
        public List<MapNodeVisual> NeighbourNodes;

        private MapNode NodeCached;
        
        #region UnityOverrides
        private void Awake()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
                EditorAwake();
            else
            #endif
                RuntimeAwake();
        }
        
        private void Start()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
                EditorStart();
            else
            #endif
                RuntimeStart();
        }
        
        private void Update()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
                EditorUpdate();
            else
            #endif
                RuntimeUpdate();
        }
        #endregion UnityOverrides
        
        #region Editor
        #if UNITY_EDITOR
        private void EditorAwake()
        {
            transform.SetParent(GameObject.Find("/MapNodes").transform);
        }

        private void EditorStart()
        {
            if (name == "MapNode")
            {
                name = GameObjectUtility.GetUniqueNameForSibling(transform.parent, "MapNode");
            }
        }

        private void EditorUpdate()
        {
            NodeNameCanvas.worldCamera = Camera.main;
            NodeNameText.text = name;
            NodeImage.sprite = MapPrefabsConfig.Get().MapNodeIcons.Find(x => x.Type == Type)?.Icon;
            
            NeighbourNodes.Remove(this);
            NeighbourNodes.RemoveAll(node => node == null);
            NeighbourNodes = NeighbourNodes.Distinct().ToList();
        }
        
        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
                return;
            
            foreach (var node in NeighbourNodes)
            {
                Vector3 from = transform.position;
                Vector3 to = node.transform.position;
                Handles.DrawAAPolyLine(5f, from, to);
            }
        }
        #endif
        #endregion Editor

        #region Runtime
        private void RuntimeAwake()
        {
            
        }

        private void RuntimeStart()
        {
            NodeNameText.text = name;
            
            foreach (var node in NeighbourNodes)
            {
                MapUtils.DrawLine(transform.position, node.transform.position);
            }
        }

        private void RuntimeUpdate()
        {
            
        }
        #endregion Runtime
    }
}