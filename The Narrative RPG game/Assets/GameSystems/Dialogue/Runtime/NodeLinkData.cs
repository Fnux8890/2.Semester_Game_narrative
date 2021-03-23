using System;

namespace GameSystems.Dialogue.Runtime
{
    [Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGUID;
        public string PortName;
        public string TargetNodeGUID;
    }
}