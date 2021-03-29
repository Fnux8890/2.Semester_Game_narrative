using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.Dialogue;
using UnityEditor;
using UnityEngine;

namespace GameSystems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public TextAsset json;
        private Dialogue_Json_Classes.Dialogue[] _dialogueArray;
        private Dialogue_Json_Classes.Dialogue _dialogue;
        private ArrayList characterArrayList = new ArrayList();
        void Start()
        {
            _dialogueArray = JsonHelper.getJsonArray<Dialogue_Json_Classes.Dialogue>(json.text);
            _dialogue = _dialogueArray[0];
            GETCharacterName();
            

        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.H))
            {
                foreach (var node in _dialogue.nodes)
                {
                    if (node.character==null)continue;
                    Debug.Log($"Node Character: {node.character} \n" +
                              $"character id: {node.characterIndex}\n" +
                              $" NodeId: {node.node_name}");
                }
            }
        }

        private void GETCharacterName()
        {
            int counter = 0;  
            string characterStuff;
            bool insideArray = false;
            System.IO.StreamReader file =
                new System.IO.StreamReader(AssetDatabase.GetAssetPath(json));
            while((characterStuff = file.ReadLine()) != null)  
            {
                counter++; 
                if (insideArray)
                {
                    if (characterStuff.Contains("]"))
                    {
                        insideArray = false;
                        continue;
                    }
                    characterArrayList.Add(characterStuff.Trim(' ', '\"', ','));
                }
                if (characterStuff.Contains("character") && characterStuff.Contains("["))
                {
                    insideArray = true;
                }
            }
            file.Close();
            Queue<string> charterName = new Queue<string>();
            Queue<int> charterIndex = new Queue<int>();
            for (int i = 0; i < characterArrayList.Count; ++i)
            {
                if (i%2==0)
                {
                    try
                    {
                        charterName.Enqueue(characterArrayList[i].ToString());
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


