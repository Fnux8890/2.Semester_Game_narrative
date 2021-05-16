using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using PlayerControl;
using DialogueClass = GameSystems.Dialogue.Dialogue_Json_Classes.Dialogue;
using UnityEngine;
using Utilities;

namespace GameSystems.Dialogue
{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        private TextAsset _json;
        public DialogueClass Dialogue { get; private set; }
        public Connections[] Connections => Dialogue.connections;
        public List<Node> Nodes => Dialogue.nodes;
        private Node _currentNode;
        private Dictionary<string, Variables> _globalVariables = new Dictionary<string, Variables>();
        private Variables _currentVariable;
        private bool _endNode = false;

        private void Start()
        {
            InteractionHandler.Instance.Interact += () => StartCoroutine(ShowDialogue());
            InteractionHandler.Instance.UpdateNode += ChoiceUpdateNode;
        }
        

        private void ChoiceUpdateNode(Node newNode)
        {
            _currentNode = Nodes.Find(x => x.node_name == newNode.node_name);
            StartCoroutine(ShowDialogue());
        }

        private IEnumerator ShowDialogue()
        {
            _currentNode ??= Nodes.Find(x => x.node_name == "START");
            if (_currentNode!=null)
            {
                while (_currentNode.NodeType != NodeTypes.ShowMessage)
                {
                    if (_currentNode==null) yield break;
                    switch (_currentNode.NodeType)
                    {
                        case NodeTypes.Execute:
                            yield return StartCoroutine(HandleNodeExecute());
                            if (_currentNode!=null) continue;
                            yield break;
                        case NodeTypes.ConditionBranch:
                            yield return StartCoroutine(HandleNodeConditions());
                            if (_currentNode!=null) continue;
                            yield break;
                        case NodeTypes.SetLocalVariable:
                            yield return StartCoroutine(SetVariable());
                            if (_currentNode!=null) continue;
                            yield break;
                        case NodeTypes.Wait:
                            yield return StartCoroutine(Wait(_currentNode));
                            if (_currentNode!=null) continue;
                            yield break;
                        case NodeTypes.Start:
                            GetNextNode();
                            break;
                        default:
                            GetNextNode();
                            break;
                    }
                }
                DialogueUIManager.Instance.DisplayDialogue(_currentNode);
                GetNextNode();
            }
        }

        private void GetNextNode()
        {
            if (_endNode)
            {
                DialogueUIManager.Instance.DisplayDialogue(_currentNode);
                _endNode = false;
                return;
            }
            if (_currentNode.HasNextNode || _currentNode.branches!=null || _currentNode.choices != null)
            {
                _currentNode = Nodes.Find(x => x.node_name == _currentNode.next);
                return;
            }
            
            if (_currentNode.HasNextNode == false && _currentNode.branches==null && _currentNode.choices == null)
            {
                _currentNode = null;
                DialogueUIHandler.Instance.OnExitDialogue();
                _endNode = false;
            }
        }

        private IEnumerator Wait(Node currentNode)
        {
            PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Disable();
            yield return new WaitForSeconds(currentNode.time);
            GetNextNode();
            PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Enable();
        }

        private IEnumerator SetVariable()
        {
            yield return _currentVariable.variables[_currentNode.var_name].VariableData = _currentNode.value;
            GetNextNode();
        }

        private IEnumerator HandleNodeExecute()
        {
            try
            {
                var variableName = _currentNode.Text.Substring(
                    _currentNode.Text.IndexOf("\\", StringComparison.Ordinal),
                    _currentNode.Text.LastIndexOf("\\", StringComparison.Ordinal) 
                    - _currentNode.Text.IndexOf("\\", StringComparison.Ordinal)).Trim('\"', '\\');
                var variable = _currentNode.Text.Substring(
                    _currentNode.Text.LastIndexOf(",", StringComparison.Ordinal),
                    _currentNode.Text.Length 
                    - _currentNode.Text.IndexOf(",", StringComparison.Ordinal));
                Debug.Log($"{variableName} = {variable}");
                var type = _currentVariable.variables[variableName].VariableData.GetType();
                dynamic variableWithType = type switch
                {
                    bool b => bool.Parse(variable),
                    string s => variable,
                    int i => int.Parse(variable),
                    _ => null
                };
                if (_currentVariable.variables[variableName].VariableData != variableWithType)
                {
                    _currentVariable.variables[variableName].VariableData = variableWithType;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("No such variable available");
                Debug.Log(e);
                throw;
            }
            
            GetNextNode();
            yield break;
        }

        private IEnumerator HandleNodeConditions()
        {
            var variable = _currentNode.Text.Substring(
                _currentNode.Text.IndexOf("\\", StringComparison.Ordinal),
                _currentNode.Text.LastIndexOf("\\", StringComparison.Ordinal) 
                           - _currentNode.Text.IndexOf("\\", StringComparison.Ordinal)).Trim('\"', '\\');
            var variableData = _currentVariable.variables[variable].VariableData;
            var nextBranch = _currentNode.branches[variableData.ToString()];
            _currentNode = _currentNode = Nodes.Find(x => x.node_name == nextBranch);
            if (!_currentNode.HasNextNode) _endNode = true;
            yield break;
        }


        public void LoadJson(TextAsset json)
        {
            _json = json;
            var df = new DialogueFetcher(_json);
            Dialogue = df.Dialogue;
            _currentNode = Nodes.Find(x => x.node_name == "START");
            if (!_globalVariables.ContainsKey(json.name))
            {
                _globalVariables.Add(json.name, Dialogue.Variables);
            }

            _currentVariable = _globalVariables[json.name];
        }

        public void UnloadJson()
        {
            _json = null;
            Dialogue = null;
            _currentNode = null;
            _currentVariable = null;
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