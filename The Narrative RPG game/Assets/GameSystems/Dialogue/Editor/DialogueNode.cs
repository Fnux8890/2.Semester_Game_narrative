using UnityEditor.Experimental.GraphView;

namespace GameSystems.Dialogue
{
    public class DialogueNode : Node
    {
        public string GUID;

        public string DialogueText;

        public bool EntryPoint = false;
    }
}