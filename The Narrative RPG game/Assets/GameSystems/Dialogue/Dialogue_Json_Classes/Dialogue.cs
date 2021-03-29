using System;
using System.Collections;
using System.Collections.Generic;

namespace GameSystems.Dialogue.Dialogue_Json_Classes
{
    [Serializable]
    public class Dialogue
    {
        public List<Node> nodes;
        
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
        #region NodeFields
        
        public string character;
        public int characterIndex;
        public Branches[] branches;
        public Choices[] choices;
        public int chance_1;
        public int chance_2;
        public string filename;
        public bool is_box;
        public int next;
        public string node_name;
        public string node_type;
        public string object_path;
        public int[] offset;
        public int time;
        public bool slide_camera;
        public int speaker_type;
        public NodeText text;

        #endregion
        
        

        
    }
    
    [Serializable]
    public class Choices{
        public string condition;
        public bool is_condition;
        public string next;
        public NodeText[] text;
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
        public string False;
        public string True;
    }
}