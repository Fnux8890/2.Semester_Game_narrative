using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dialogue;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEditor;
using UnityEngine;

namespace GameSystems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextAsset json;
        [SerializeField] public GameObject dialogueBox;
        private Dialogue_Json_Classes.Dialogue _dialogue;
        private SpeakerUI _speakerUI;

        private void Start()
        {
            DialogueSetup();

            _speakerUI = dialogueBox.GetComponent<SpeakerUI>();
        }

        private async void DialogueSetup()
        {
            var dialogueArray = JsonHelper.GETJsonArray<Dialogue_Json_Classes.Dialogue>(json.text);
            _dialogue = dialogueArray[0];
            var filePath = AssetDatabase.GetAssetPath(json);
            await GETInconsistentlyDataAsync(filePath);
            var sorted = _dialogue.nodes
                .OrderBy(x => x.NodeIndex).ToList();
        }
        

        private async Task GETInconsistentlyDataAsync(string filePath)
        {
            var tasks = new List<Task>
            {
                Task.Run(() => GETVariablesAsync(filePath)),
                Task.Run(() => GETBranchesAsync(filePath)),
                Task.Run(() => GETCharacterNameAsync(filePath))
            };

            await Task.WhenAll(tasks);
        }

        private void GETVariablesAsync(string filePath)
        {
            if (_dialogue == null) return;
            string readLine;
            var insideVariables = false;
            _dialogue.Variables.variables = new Dictionary<string, Variable>();
            var file = new StreamReader(filePath);
            var insideCount = 0;
            var numberOfObjects = 0;
            var keys = new List<string>();
            var types = new List<int>();
            var values = new ArrayList();
            while ((readLine = file.ReadLine()) != null)
            {
                if (readLine.Contains("variables"))
                {
                    insideVariables = true;
                }

                if (insideVariables)
                {
                    if (readLine.Contains("{") && readLine.Contains("}"))
                    {
                        var variableKey = readLine.Substring(0, readLine.IndexOf(
                            ":", StringComparison.Ordinal)).Trim('\"', ' ');
                        var insideObject = readLine.Substring(
                            readLine.IndexOf("{", StringComparison.Ordinal),
                            readLine.IndexOf("}", StringComparison.Ordinal) + 1
                            - readLine.IndexOf("{", StringComparison.Ordinal));
                        var indexFrom = new List<int>();
                        var indexTo = new List<int>();
                        var index = 0;
                        while ((index = insideObject.IndexOf(":", index, StringComparison.Ordinal)) != -1)
                        {
                            indexFrom.Add(index);
                            index++;
                        }

                        index = 0;
                        while ((index = insideObject.IndexOf(",", index, StringComparison.Ordinal)) != -1)
                        {
                            indexTo.Add(index);
                            index++;
                        }

                        if (indexFrom.Count > indexTo.Count)
                        {
                            var indexToAdd = insideObject.IndexOf("}", StringComparison.Ordinal);
                            indexTo.Add(indexToAdd);
                        }

                        if (indexFrom.Count == indexTo.Count)
                        {
                            int.TryParse(insideObject
                                .Substring(indexFrom[0], indexTo[0] - indexFrom[0]).Trim(' ', ':'), out var type);
                            switch (type)
                            {
                                case 0:
                                    _dialogue.Variables.variables
                                        .Add(variableKey, new Variable(type,
                                            insideObject
                                                .Substring(indexFrom[1], indexTo[1] - indexFrom[1])
                                                .Trim(' ', '\"', ',', '}')));
                                    break;
                                case 1:
                                    int.TryParse(insideObject
                                        .Substring(indexFrom[1], indexTo[1] - indexFrom[1])
                                        .Trim(' ', '\"', ',', '}'), out var valueInt);
                                    _dialogue.Variables.variables
                                        .Add(variableKey, new Variable(type, valueInt));
                                    break;
                                case 2:
                                    bool.TryParse(insideObject
                                        .Substring(indexFrom[1], indexTo[1] - indexFrom[1])
                                        .Trim(' ', '\"', ',', '}'), out var valueBool);
                                    _dialogue.Variables.variables
                                        .Add(variableKey, new Variable(type, valueBool));
                                    break;
                            }
                        }

                        continue;
                    }

                    if (readLine.Contains("{") && !readLine.Contains("}")) insideCount++;
                    if (readLine.Contains("}") && !readLine.Contains("{"))
                    {
                        insideCount--;
                        numberOfObjects++;
                    }

                    if (insideCount == 0)
                    {
                        insideVariables = false;
                    }


                    if (insideVariables && readLine.Contains(":") && insideCount > 0 && !readLine.Contains("variables"))
                    {
                        if (readLine.Contains("{"))
                        {
                            keys.Add(readLine.Substring(0, readLine.IndexOf(
                                ":", StringComparison.Ordinal)).Trim('\"', ' '));
                        }

                        if (readLine.Contains("type") && readLine.Contains(","))
                        {
                            var stringToInt = readLine
                                .Substring(readLine.IndexOf(":", StringComparison.Ordinal),
                                    readLine.IndexOf(",", StringComparison.Ordinal)
                                    - readLine.IndexOf(":", StringComparison.Ordinal) + 1)
                                .Trim(' ', ',', ':');
                            int.TryParse(stringToInt, out int type);
                            types.Add(type);
                        }

                        if (readLine.Contains("value"))
                        {
                            switch (types[numberOfObjects])
                            {
                                case 0:
                                    _dialogue.Variables.variables
                                        .Add(keys[numberOfObjects], new Variable(types[numberOfObjects],
                                            readLine
                                                .Substring(readLine.IndexOf(":", StringComparison.Ordinal),
                                                    readLine.Length - readLine
                                                        .IndexOf(":", StringComparison.Ordinal))
                                                .Trim(' ', '\"', ',', '}')));
                                    break;
                                case 1:
                                    int.TryParse(readLine
                                        .Substring(readLine.IndexOf(":", StringComparison.Ordinal),
                                            readLine.Length - readLine
                                                .IndexOf(":", StringComparison.Ordinal))
                                        .Trim(' ', '\"', ',', '}'), out var valueInt);
                                    _dialogue.Variables.variables
                                        .Add(keys[numberOfObjects], new Variable(types[numberOfObjects], valueInt));
                                    break;
                                case 2:
                                    var stringToBool = readLine
                                        .Substring(readLine.IndexOf(":", StringComparison.Ordinal),
                                            readLine.Length - readLine
                                                .IndexOf(":", StringComparison.Ordinal))
                                        .Trim(' ', '\"', ',', '}', ':');
                                    bool.TryParse(stringToBool, out var valueBool);
                                    _dialogue.Variables.variables
                                        .Add(keys[numberOfObjects], new Variable(types[numberOfObjects], valueBool));
                                    break;
                            }
                        }
                    }
                }
            }

            file.Close();
        }

        private void GETBranchesAsync(string filePath)
        {
            #region SeachFileForBranches

            if (_dialogue == null)
            {
                return;
            }

            var file = new StreamReader(filePath);
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
                            - readLine.IndexOf("{", StringComparison.Ordinal));
                        var indexFrom = new List<int>();
                        var indexTo = new List<int>();
                        var index = 0;
                        while ((index = insideObject.IndexOf(":", index, StringComparison.Ordinal)) != -1)
                        {
                            indexFrom.Add(index);
                            index++;
                        }

                        index = 0;
                        while ((index = insideObject.IndexOf(",", index, StringComparison.Ordinal)) != -1)
                        {
                            indexTo.Add(index);
                            index++;
                        }

                        if (indexFrom.Count == indexTo.Count)
                        {
                            _dialogue.nodes[nodeNumber - 1].branches.branchesString = indexFrom.Select(
                                (t, i) => insideObject.Substring(t, indexTo[i] - 1 - t)
                                    .Trim(':', ' ', '\"')).ToList();
                        }
                    }
                }
            }

            file.Close();

            #endregion
        }

        private void GETCharacterNameAsync(string filePath)
        {
            #region SeachFileForCharacters

            if (_dialogue == null)
            {
                return;
            }

            string readLine;
            var insideArray = false;
            var insideNode = false;
            var file = new StreamReader(filePath);
            ArrayList characterArrayList = new ArrayList();
            while ((readLine = file.ReadLine()) != null)
            {
                if (readLine.Contains("nodes")) insideNode = true;
                if (insideNode)
                {
                    if (insideArray)
                    {
                        if (readLine.Contains("]"))
                        {
                            insideArray = false;
                            continue;
                        }

                        characterArrayList.Add(readLine.Trim(' ', '\"', ','));
                    }

                    if (readLine.Contains("character") && readLine.Contains("[") &&
                        !readLine.Contains("]"))
                    {
                        insideArray = true;
                    }

                    if (readLine.Contains("[") && readLine.Contains("]") &&
                        readLine.Contains("character"))
                    {
                        var substringList = readLine.Substring(
                                readLine.IndexOf("[", StringComparison.Ordinal),
                                ((readLine.IndexOf("]", StringComparison.Ordinal) + 1)
                                 - readLine.IndexOf("[", StringComparison.Ordinal)))
                            .Trim(' ', '\"', '[', ']').Split(',').ToList();
                        characterArrayList.AddRange(substringList);
                    }
                }
            }

            file.Close();

            #endregion

            #region InsertCharactersToNodes

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

            #endregion
        }
    }
}