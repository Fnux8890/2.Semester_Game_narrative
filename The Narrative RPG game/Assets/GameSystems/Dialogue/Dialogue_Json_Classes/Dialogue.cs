using System;
using System.Collections;
using System.Collections.Generic;

namespace Systems.Dialogue.Dialogue_Json_Classes
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

        public ArrayList character;
        private Branches[] branches;
        private Choices[] choices;
        private int chance_1;
        private int chance_2;
        private string filename;
        private bool is_box;
        private int next;
        public string node_name;
        private string node_type;
        private string object_path;
        private int[] offset;
        private int time;
        private bool slide_camera;
        private int speaker_type;
        private NodeText text;

        #endregion
        
        #region NodePropeties

        public ArrayList Character
        {
            get => character;
            set => character = value;
        }

        public Branches[] Branches
        {
            get => branches;
            set => branches = value;
        }

        public Choices[] Choices
        {
            get => choices;
            set => choices = value;
        }

        public int Chance1
        {
            get => chance_1;
            set => chance_1 = value;
        }

        public int Chance2
        {
            get => chance_2;
            set => chance_2 = value;
        }

        public string Filename
        {
            get => filename;
            set => filename = value;
        }

        public bool IsBox
        {
            get => is_box;
            set => is_box = value;
        }

        public int Next
        {
            get => next;
            set => next = value;
        }

        public string NodeName
        {
            get => node_name;
            set => node_name = value;
        }

        public string NodeType
        {
            get => node_type;
            set => node_type = value;
        }

        public string ObjectPath
        {
            get => object_path;
            set => object_path = value;
        }

        public int[] Offset
        {
            get => offset;
            set => offset = value;
        }

        public int Time
        {
            get => time;
            set => time = value;
        }

        public bool SlideCamera
        {
            get => slide_camera;
            set => slide_camera = value;
        }

        public int SpeakerType
        {
            get => speaker_type;
            set => speaker_type = value;
        }

        public NodeText Text
        {
            get => text;
            set => text = value;
        }

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