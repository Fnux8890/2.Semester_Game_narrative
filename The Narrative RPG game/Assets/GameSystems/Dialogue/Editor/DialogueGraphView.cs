using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Dialogue.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameSystems.Dialogue
{
    public class DialogueGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(150,200);

        private NodeSearchWindow _searchWindow;
        
        public DialogueGraphView(EditorWindow editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var gridBackground = new GridBackground();
            Insert(0,gridBackground);
            gridBackground.StretchToParentSize();

            AddElement(GenerateEntryPointNode());
            AddSearchWindow(editorWindow);
        }

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Init(editorWindow,this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition),_searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            
            ports.ForEach((port) =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        private Port GeneratePort(DialogueNode node, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, direction, capacity,typeof(float));
        }
        
        private DialogueNode GenerateEntryPointNode()
        {
            var node = new DialogueNode
            {
                title = "start",
                GUID = Guid.NewGuid().ToString(),
                DialogueText = "ENTRYPOINT",
                EntryPoint = true
            };

            var generatedPort = GeneratePort(node, Direction.Output);
            generatedPort.name = "Next";
            node.outputContainer.Add(generatedPort);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            node.SetPosition(new Rect(100, 200,100,150));
            return node;
        }

        public void CreateNode(string nodeName, Vector2 createPoint = default(Vector2))
        {
            AddElement(CreateDialogueNode(nodeName, createPoint));
        }
        

        public DialogueNode CreateDialogueNode(string nodeName, Vector2 createPoint = default(Vector2))
        {
            var dialogueNode = new DialogueNode
            {
                title = nodeName,
                DialogueText = nodeName,
                GUID = Guid.NewGuid().ToString()
            };

            var inputPorts = GeneratePort(dialogueNode,Direction.Input,Port.Capacity.Multi);
            inputPorts.portName = "Input";
            dialogueNode.inputContainer.Add(inputPorts);

            dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            var button = new Button(() =>
            {
                AddChoicePort(dialogueNode);
            });
            button.text = "New Choice";
            dialogueNode.titleContainer.Add(button);

            var textField = new TextField(String.Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                dialogueNode.DialogueText = evt.newValue;
                dialogueNode.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(dialogueNode.title);

            dialogueNode.mainContainer.Add(textField);

            dialogueNode.RefreshExpandedState();
            dialogueNode.RefreshPorts();
            dialogueNode.SetPosition(new Rect(createPoint, DefaultNodeSize));
            
            return dialogueNode;

        }

        public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "")
        {
            var generatedPort = GeneratePort(dialogueNode,Direction.Output);

            var oldLebel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLebel);
            
            var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;

            var choicePortName = String.IsNullOrEmpty(overriddenPortName)
                ? $"Choice {outputPortCount + 1}"
                : overriddenPortName;

            var textField = new TextField
            {
                name = string.Empty,
                value = choicePortName
            };

            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);
            var deleteButton = new Button(() => RemovePort(dialogueNode,generatedPort))
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);
            
            generatedPort.portName = choicePortName;

            dialogueNode.outputContainer.Add(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
            

        }

        private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
        {
            var targetEdge = edges.ToList().Where(x => 
                x.output.portName==generatedPort.portName && x.output.node == generatedPort.node);
            if (!targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            dialogueNode.outputContainer.Remove(generatedPort);
            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
        }
    }
}