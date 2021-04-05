using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Dialogue.Dialogue_Json_Classes
{
    [Serializable]
    public class Dialogue
    {
        public string[] characters;
        public Connections[] connections;
        public string file_name;
        public string[] language;
        public List<Node> nodes;
        public string selected_language;
        public Variables variables;
    }

    [Serializable]
    public class Variables
    {
        public AlreadyMet already_met;
        public SeenFireworks seen_fireworks;
    }

    [Serializable]
    public class AlreadyMet
    {
        public int type;
        public bool value;
    }

    [Serializable]
    public class SeenFireworks
    {
        public int type;
        public bool value;
    }


    [Serializable]
    public class DialogueOld
    {
        public string[] characters;
        public Connections[] connections;
        public string editor_version;
        public string file_name;
        public string[] languages;
        public Node[] nodes;
        public string selected_language;
    }

    [Serializable]
    public class Connections
    {
        public string from;
        public int from_port;
        public string to;
        public int to_port;
    }


    [Serializable]
    public class Node
    {
        #region NodeFieldsFromJson

        public string character;
        public int characterIndex;
        public Branches branches;
        public Choices[] choices;
        public int chance_1;
        public int chance_2;
        public string filename;
        public bool is_box;
        public string next;
        public int next_index;
        public string node_name;
        public int node_index;
        public string node_type;
        public string object_path;
        public int[] offset;
        public int time;
        public bool slide_camera;
        public int speaker_type;
        public NodeText text;

        #endregion
        
        public string Text => text.ENG;
        public int NodeIndex
        {
            get
            {
                if (next != null && node_name.ToLower().Contains("start"))
                {
                    return 1;
                }
                if (node_name != null && int.TryParse(node_name,out node_index))
                {
                    return node_index;
                }

                return 0;
            }
        }
        public int NextIndex
        {
            get
            {
                if (next != null && next.ToLower().Contains("start"))
                {
                    return 1;
                }
                if (next != null && int.TryParse(next,out next_index))
                {
                    //Debug.Log($"{next_index} == {6061566}? {next_index == 6061566}");
                    return next_index;
                    //2399617
                }

                return 0;
            }
        }
        
    }

    public class NodeCompare : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            var nodeNameComparison = string.Compare(x.node_name, y.node_name, StringComparison.Ordinal);
            if (nodeNameComparison != 0) return nodeNameComparison;
            return x.NodeIndex.CompareTo(y.NodeIndex);
        }
    }
    
    

    [Serializable]
    public class Choices
    {
        public string condition;
        public bool is_condition;
        public string next;
        public NodeText text;
    }

    [Serializable]
    public class NodeText
    {
        public string ENG;
        public string FR;
        public string RUS;
    }

    [Serializable]
    public class Branches
    {
        public List<string> branches;
    }
    
    
    
}