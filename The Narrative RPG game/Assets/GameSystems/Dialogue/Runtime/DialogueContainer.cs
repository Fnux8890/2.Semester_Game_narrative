using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Dialogue.Runtime
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
    }
}