using System;
using System.Collections.Generic;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using DialogueClass = GameSystems.Dialogue.Dialogue_Json_Classes.Dialogue;
using TC = GameSystems.Dialogue.TreeCollection.TreeCollection;
using UnityEngine;
using Utilities;

namespace GameSystems.Dialogue
{
    public class DialogueManager
    {
        private TextAsset _json;
        
        public DialogueClass Dialogue { get; private set; }
        public Connections[] Connections => Dialogue.connections;
        public List<Node> Nodes => Dialogue.nodes;
        
        public TC Tc {get; private set;}
        
        
        public DialogueManager(TextAsset json)
        {
            _json = json;
            var df = new DialogueFetcher(_json);
            Dialogue = df.Dialogue;
            //Tc = PopulateTree();
            
        }

        private TC PopulateTree()
        {
            var tc = new TC();
            var rootConnection = Array.Find(Connections, x => x.@from == "START");
            var rootNode = Nodes.Find(node => node.node_name == rootConnection.@from);
            Node node;
            while ((node = InsertRecursive(rootNode)) != null)
            {
                tc.Insert(node);
            }
            return tc;
        }

        private Node InsertRecursive(Node node)
        {
            if (node.next == null && node.branches == null) return null;
            if (node.next != null)
            {
                InsertRecursive(FindNode(node.next));
            }

            if (node.branches != null)
            {
                Debug.Log("SELECT");
            }
            return node;
        }

        private Node FindNode(string nodeName)
        {
            return Nodes.Find(x => x.node_name == nodeName);
        }
    }
}