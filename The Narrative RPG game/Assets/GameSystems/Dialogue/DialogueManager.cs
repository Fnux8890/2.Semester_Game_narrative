using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dialogue;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using GameSystems.Dialogue.DialogueGraphStructure;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameSystems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextAsset json;
        private Dialogue_Json_Classes.Dialogue[] _dialogueArray;
        private Dialogue_Json_Classes.Dialogue _dialogue;
        [SerializeField] public GameObject dialogueBox;
        private SpeakerUI _speakerUI;
        private int _index = 0;

        private void Start()
        {
            DialogueSetup();
            _speakerUI = dialogueBox.GetComponent<SpeakerUI>();

            // used for testing
            //var jsonFrom = JsonUtility.ToJson(_dialogue);
            //Debug.Log(jsonFrom);
        }

        private void DialogueSetup()
        {
            _dialogueArray = JsonHelper.GETJsonArray<Dialogue_Json_Classes.Dialogue>(json.text);
            _dialogue = _dialogueArray[0];
            GETCharacterName();
            GETBranches();
            var nodeCompare = new NodeCompare();
            var sorted = _dialogue.nodes
                .OrderBy(x => x.NodeIndex).ToList();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var item = sorted.Find(x=> x.branches.branchesString != null && x.branches.BranchesInt.Contains(6061566));
            sw.Stop();
            Debug.Log(string.Join(" | ", item.NodeIndex));
        }
        
        private void Update()
        {
            
            if (Input.GetKeyUp(KeyCode.H))
            {
                while (_dialogue.nodes[_index].Text == null)
                {
                    _index++;
                }
                _speakerUI.dialogue.text = _dialogue.nodes[_index].NodeIndex.ToString();
                _index++;
            }
        }
        
        private void GETBranches()
        {
            if (_dialogue == null)
            {
                return;
            }
            var file =
                new System.IO.StreamReader(AssetDatabase.GetAssetPath(json));
            string readLine;
            var insideNodes = false;
            var nodeNumber = 0;
            var insideChoices = false;
            var insideNode = false;
            while ((readLine = file.ReadLine()) != null)
            {
                if (readLine.Contains("nodes")) insideNodes = true;
                if (insideNodes)
                {
                    if (readLine.Contains("{") && readLine.Trim(' ').Length < 2)
                    {
                        insideNode = true;
                        nodeNumber++;
                    }
                    if (readLine.Contains("choices") && readLine.Contains("["))
                    {
                        insideChoices = true;
                    }
                    if (readLine.Contains("],") && insideNode)
                    {
                        insideChoices = false;
                    }

                    if (readLine.Contains("},") && !insideChoices)
                    {
                        insideNode = false;
                    }
                    if (readLine.Contains("branches") && readLine.Contains("}"))
                    {
                        var insideObject = readLine.Substring(
                            readLine.IndexOf("{", StringComparison.Ordinal),
                            readLine.IndexOf("}", StringComparison.Ordinal) + 2
                            - readLine.IndexOf("{", StringComparison.Ordinal) );
                        var indexFrom = new List<int>();
                        var indexTo = new List<int>();
                        var index = 0;
                        while ((index = insideObject.IndexOf(":",index, StringComparison.Ordinal)) != -1)
                        {
                            indexFrom.Add(index);
                            index++;
                        }

                        index = 0;
                        while ((index = insideObject.IndexOf(",",index, StringComparison.Ordinal)) != -1)
                        {
                            indexTo.Add(index);
                            index++;
                        }

                        if (indexFrom.Count == indexTo.Count)
                        {
                            _dialogue.nodes[nodeNumber-1].branches.branchesString = indexFrom.Select((t, i) => insideObject.Substring(t, indexTo[i] - 1 - t).Trim(':', ' ', '\"')).ToList();
                        }
                    }
                }
            }
            file.Close();

            //Debug.Log(_dialogue.nodes.Count);
            //Debug.Log(nodeNumber);
        }


        private void GETCharacterName()
        {
            if (_dialogue == null)
            {
                return;
            }

            string characterStuff;
            var insideArray = false;
            var insideNode = false;
            ArrayList characterArrayList = new ArrayList();
            var file =
                new System.IO.StreamReader(AssetDatabase.GetAssetPath(json));
            while ((characterStuff = file.ReadLine()) != null)
            {
                if (characterStuff.Contains("nodes")) insideNode = true;
                if (insideNode)
                {
                    if (insideArray)
                    {
                        if (characterStuff.Contains("]"))
                        {
                            insideArray = false;
                            continue;
                        }

                        characterArrayList.Add(characterStuff.Trim(' ', '\"', ','));
                    }

                    if (characterStuff.Contains("character") && characterStuff.Contains("[") &&
                        !characterStuff.Contains("]"))
                    {
                        insideArray = true;
                    }

                    if (characterStuff.Contains("[") && characterStuff.Contains("]") &&
                        characterStuff.Contains("character"))
                    {
                        var substringList = characterStuff.Substring(
                                characterStuff.IndexOf("[", StringComparison.Ordinal),
                                ((characterStuff.IndexOf("]", StringComparison.Ordinal) + 1)
                                 - characterStuff.IndexOf("[", StringComparison.Ordinal)))
                            .Trim(' ', '\"', '[', ']').Split(',').ToList();
                        characterArrayList.AddRange(substringList);
                    }
                }
            }

            file.Close();
            var charterName = new Queue<string>();
            var charterIndex = new Queue<int>();
            for (int i = 0; i < characterArrayList.Count; ++i)
            {
                if (i % 2 == 0)
                {
                    try
                    {
                        charterName.Enqueue(characterArrayList[i].ToString().Trim('\"'));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        int.TryParse(characterArrayList[i].ToString(), out var index);
                        charterIndex.Enqueue(index);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            foreach (var node in _dialogue.nodes.Where(node => node.text.ENG != null))
            {
                node.character = charterName.Dequeue();
                node.characterIndex = charterIndex.Dequeue();
            }
        }
    }
}