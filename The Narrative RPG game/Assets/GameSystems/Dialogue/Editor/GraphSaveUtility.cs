using System.Collections.Generic;
using System.Linq;
using GameSystems.Dialogue.Runtime;
using Subtegral.DialogueSystem.DataContainers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace GameSystems.Dialogue.Editor
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _containerCash;
        
        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
        private List<Group> CommentBlocks =>
            _targetGraphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();
        
        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
            if (!SaveNodes(dialogueContainer)){return;}

            SaveExposedProprieties(dialogueContainer);
            SaveCommentBlocks(dialogueContainer);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/DialogueGraphData"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "DialogueGraphData");
            }
            
            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/DialogueGraphData/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        private void SaveCommentBlocks(DialogueContainer dialogueContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID)
                    .ToList();

                dialogueContainer.CommentBlockData.Add(new CommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }

        private void SaveExposedProprieties(DialogueContainer dialogueContainer)
        {
            dialogueContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
        }

        private bool SaveNodes(DialogueContainer dialogueContainer)
        {
            if (!Edges.Any()) return false;
            

            var connectedPorts = Edges.Where(x => x.input.node!=null).ToArray();
            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as DialogueNode;
                var inputNode = connectedPorts[i].input.node as DialogueNode;

                dialogueContainer.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedPorts[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var dialogueNode in Nodes.Where(node=> !node.EntryPoint))
            {
                dialogueContainer.DialogueNodeDatas.Add(new DialogueNodeData
                {
                    NodeGUID = dialogueNode.GUID,
                    DialogueText = dialogueNode.DialogueText,
                    Position = dialogueNode.GetPosition().position
                });
            }

            return true;
        }
        
        public void LoadGraph(string fileName)
        {
            _containerCash = Resources.Load<DialogueContainer>(fileName);
            if (_containerCash == null)
            {
                EditorUtility.DisplayDialog("File not found", "Target dialogue graph file does not exists!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
            CreateExposedProperties();
            GenerateCommentBlocks();
        }

        private void CreateExposedProperties()
        {
            _targetGraphView.ClearBlackboardAndExpressedProperties();
            foreach (var exposedProperty in _containerCash.ExposedProperties)
            {
                _targetGraphView.AddPropertyToBlackboard(exposedProperty);
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(x => x.EntryPoint).GUID = _containerCash.NodeLinks[0].BaseNodeGUID;

            foreach (var node in Nodes)
            {
                if(node.EntryPoint) continue;
                Edges.Where(x => x.input.node == node).ToList()
                    .ForEach(edge => _targetGraphView.RemoveElement(edge));
                _targetGraphView.RemoveElement(node);
            }
            
        }
        
        private void CreateNodes()
        {
            foreach (var nodeData in _containerCash.DialogueNodeDatas)
            {
                var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
                tempNode.GUID = nodeData.NodeGUID;
                _targetGraphView.AddElement(tempNode);
                
                var nodePorts = _containerCash.NodeLinks.Where(x => x.BaseNodeGUID== nodeData.NodeGUID).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));

            }
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var connections = _containerCash.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[i].GUID).ToList();
                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGUID = connections[j].TargetNodeGUID;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                    LinkNoed(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);
                    
                    targetNode.SetPosition(
                        new Rect(_containerCash.DialogueNodeDatas.First(x=>x.NodeGUID==targetNodeGUID).Position,
                            _targetGraphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNoed(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };
            
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }
        
        private void GenerateCommentBlocks()
        {
            foreach (var commentBlock in CommentBlocks)
            {
                _targetGraphView.RemoveElement(commentBlock);
            }

            foreach (var commentBlockData in _containerCash.CommentBlockData)
            {
                var block = _targetGraphView.CreateCommentBlock(new Rect(commentBlockData.Position, _targetGraphView.DefaultCommentBlockSize),
                    commentBlockData);
                block.AddElements(Nodes.Where(x=>commentBlockData.ChildNodes.Contains(x.GUID)));
            }
        }
        
    }
}