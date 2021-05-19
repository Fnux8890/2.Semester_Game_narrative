using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using PlayerControl;
using DialogueClass = GameSystems.Dialogue.Dialogue_Json_Classes.Dialogue;
using UnityEngine;
using Utilities;
using Random = System.Random;

namespace GameSystems.Dialogue
{
    
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        private TextAsset _json;
        private DialogueClass Dialogue { get; set; }
        private Connections[] Connections => Dialogue.connections;
        public List<Node> Nodes => Dialogue.nodes;
        private Node _currentNode;
        private Dictionary<string, Variables> _globalVariables = new Dictionary<string, Variables>();
        private Variables _currentVariable;
        private Node _lastNode;
        private bool _endNodeRan = false;
        private bool _isCutscene = false;

        private void Start()
        {
            InteractionHandler.Instance.Interact += () => StartCoroutine(ShowDialogue());
            InteractionHandler.Instance.UpdateNode += ChoiceUpdateNode;
            InteractionHandler.Instance.StartCutscene += StartCutscene;
        }

        private void StartCutscene(TextAsset json)
        {
            PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Disable();
            _isCutscene = true;
            LoadJson(json);
            StartCoroutine(ShowDialogue());
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
                            StartCoroutine(GetNextNode());
                            break;
                        case NodeTypes.ChanceBranch:
                            StartCoroutine(HandleChance());
                            break;
                        case NodeTypes.RandomBranch:
                            StartCoroutine(HandleRandom());
                            break;
                        case NodeTypes.Repeat:
                            StartCoroutine(HandleRepeat());
                            break;
                        default:
                            StartCoroutine(GetNextNode());
                            break;
                    }
                }
                yield return StartCoroutine(GetNextNode());
            }
        }

        
        private IEnumerator HandleRepeat()
        {
            if (_currentNode.value == 0)
            {
                _currentNode = Nodes.Find(x => x.node_name == _currentNode.next_done);
                yield break;
            }
            else
            {
                int nodeValue = _currentNode.value;
                nodeValue--;
                _currentNode.value = nodeValue;
                _currentNode = Nodes.Find(x => x.node_name == _currentNode.next);
                yield break;
            }
        }

        
        private IEnumerator HandleRandom()
        {
            var rnd = new Random();
            var result = rnd.Next(1,_currentNode.possibilities+1);
            var branch = _currentNode.branches[result.ToString()];
            _currentNode = Nodes.Find(x => x.node_name == branch);
            yield break;
        }

        
        private IEnumerator HandleChance()
        {
            StartCoroutine(GetNextNode());
            yield break;
        }

        private IEnumerator GetNextNode()
        {
            var currentNodeIsLast = Nodes.Find(x => x.node_name == _currentNode.next).node_name == _lastNode.node_name;
            if (_currentNode.NodeType == NodeTypes.ShowMessage && !currentNodeIsLast)
            {
                yield return DialogueUIHandler.Instance.OnShowDialogue(_currentNode);
            }
            if (!currentNodeIsLast ||  _currentNode.branches!=null || _currentNode.choices != null)
            {
                _currentNode = Nodes.Find(x => x.node_name == _currentNode.next);
                yield break;
            }
            if (_endNodeRan)
            {
                CloseDialogue();
                yield break;
            }
            if (currentNodeIsLast)
            {
                yield return DialogueUIHandler.Instance.OnShowDialogue(_currentNode);
                _endNodeRan = true;
                yield break;
            }
            
            
        }

        private void CloseDialogue()
        {
            _currentNode = null;
            DialogueUIHandler.Instance.OnExitDialogue();
            if (_isCutscene)
            {
                foreach (var canvas in Resources.FindObjectsOfTypeAll<Canvas>())
                {
                    if (canvas.name == "TutorialCanvas")
                    {
                        canvas.gameObject.SetActive(true);
                        InteractionHandler.Instance.OnEndCutscene();
                    }
                }

                PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Enable();
            }

            _endNodeRan = false;
        }

        private IEnumerator Wait(Node currentNode)
        {
            PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Disable();
            yield return new WaitForSeconds(currentNode.time);
            StartCoroutine(GetNextNode());
            PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Enable();
        }

        private IEnumerator SetVariable()
        {
            yield return _currentVariable.variables[_currentNode.var_name].VariableData = _currentNode.value;
            StartCoroutine(GetNextNode());
        }

        private IEnumerator HandleNodeExecute()
        {
            var inMethodParam = false;
            var method = new StringBuilder();
            var methodPram = new StringBuilder();
            var methodParamEncounter = 0;
            char? previousCh = null;
            foreach (var ch in _currentNode.Text)
            {
                if (previousCh != null)
                {
                    if (ch == '"' && previousCh == '\\')
                    {
                        methodParamEncounter++;
                    }

                    inMethodParam = methodParamEncounter % 2 != 0;

                    if (inMethodParam)
                    {
                        methodPram.Append(ch);
                    }
                    else
                    {
                        method.Append(ch);
                    }
                }
                previousCh = ch;
            }

            var methodTrim = string.Empty;
            foreach (var ch in method.ToString())
            {
                if (ch == '\"' || ch == '\\' || ch == '(' ||  ch == ')')
                {
                    continue;
                }
                methodTrim += ch;
            }
            var methodParamTrim = methodPram.ToString().Trim('\\', '\"');
            ExecutedMethod(methodTrim.Trim(' '), methodParamTrim);

            var afterExecution = Connections.ToList().Find(x=> x.@from == _currentNode.node_name);
            if (afterExecution.to != null)
            {
                _currentNode = Nodes.Find(x => x.node_name == afterExecution.to);
                yield break;
            }

            StartCoroutine(GetNextNode());
            yield break;
        }

        private void ExecutedMethod(string method, string param)
        {
            var type = GetType();
            var theMethod = type.GetMethod(method);
            if (theMethod == null)
            {
                Debug.LogWarning("Method specified in dialogue designer not available");
                return;
            }
            theMethod?.Invoke(this, null);
        }

        public void PlaySound()
        {
            Debug.Log("You played a sound");
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
            SetLastConnectionNode();
        }

        private void SetLastConnectionNode()
        {
            foreach (var connection in Connections)
            {
                if (Connections.ToList().Find(x => x.@from == connection.to) == null)
                {
                    _lastNode = Nodes.Find(x => x.node_name == connection.to);
                };
            }
        }

        public void UnloadJson()
        {
            _json = null;
            Dialogue = null;
            _currentNode = null;
            _currentVariable = null;
        }
    }
}