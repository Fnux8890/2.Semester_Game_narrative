using System;
using System.Collections.Generic;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using DialogueClass = GameSystems.Dialogue.Dialogue_Json_Classes.Dialogue;
using UnityEngine;
using Utilities.Singleton;

namespace GameSystems.Dialogue
{
    public class DialogueMananger : Singleton<DialogueMananger>
    {
        private TextAsset _json;
        
        public DialogueClass Dialogue { get; private set; }
        public Connections[] Connections => Dialogue.connections;
        public List<Node> Nodes => Dialogue.nodes;
        
        
        
        public void LoadJson(TextAsset json)
        {
            _json = json;
            var df = new DialogueFetcher(_json);
            Dialogue = df.Dialogue;
        }

        public void UnloadJson()
        {
            _json = null;
            Dialogue = null;
        }

        private void PopulateTree()
        {
            var rootConnection = Array.Find(Connections, x => x.@from == "START");
            var rootNode = FindNode(rootConnection.@from);
        }
        

        private Node FindNode(string nodeName)
        {
            return Nodes.Find(x => x.node_name == nodeName);
        }
    }
}