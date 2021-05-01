using System.Collections.Generic;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;

namespace GameSystems.Dialogue.TreeCollection
{
    public class TreeNode
    {
        
        public Node Data { get; }
        private List<TreeNode> _nodeConnections;

        public List<TreeNode> NodeConnections
        {
            get => _nodeConnections;
            set => _nodeConnections = value;
        }
        
        public TreeNode(Node data)
        {
            Data = data;
            
        }

        public void Insert(Node node)
        {
            
        }
        
    }
}