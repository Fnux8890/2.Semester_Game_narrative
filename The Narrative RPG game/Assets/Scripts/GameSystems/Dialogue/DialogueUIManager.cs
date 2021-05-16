using System.Collections.Generic;
using System.Linq;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace GameSystems.Dialogue
{
    public class DialogueUIManager : Singleton<DialogueUIManager>
    {
        private List<GameObject> _dialogueBoxes = new List<GameObject>();

        private void Awake()
        {
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                _dialogueBoxes.Add(gameObject.transform.GetChild(i).gameObject);
            }
            DialogueUIHandler.Instance.ExitDialogue += CloseDialogue;
        }
        
        public void DisplayDialogue(Node currentNode)
        {
            BranchedDialogueOrNot(currentNode, currentNode.choices != null);
        }

        private void BranchedDialogueOrNot(Node currentNode, bool hasChoices)
        {
            HandleBox(
                currentNode: currentNode,
                isBox: currentNode.is_box,
                hasChoices: hasChoices);
        }

        private void HandleBox(Node currentNode, bool isBox, bool hasChoices)
        {
            if (isBox)
            {
                var dialogueBox = _dialogueBoxes.Find(box => box.name == "DialogueBox Top");
                dialogueBox.gameObject.SetActive(true);
                HasDecision(currentNode, hasChoices, dialogueBox, isBox);
                var dialogue = dialogueBox.transform.Find("Dialogue").GetComponent<Text>();
                dialogue.text = currentNode.Text;
            }

            if (!isBox)
            {
                GameObject dialogueBox;
                if (currentNode.character == "Player")
                {
                    dialogueBox = _dialogueBoxes.Find(box => box.name == "DialogueBoxLeft");
                    dialogueBox.transform.Find("Name").GetComponent<Text>().text = currentNode.character;
                }
                else
                {
                    dialogueBox = _dialogueBoxes.Find(box => box.name == "DialogueBoxRight");
                    dialogueBox.transform.Find("Name").GetComponent<Text>().text = currentNode.character;
                }
                dialogueBox.gameObject.SetActive(true);
                HasDecision(currentNode,hasChoices, dialogueBox, isBox);
                var dialogue = dialogueBox.transform.Find("Dialogue").GetComponent<Text>();
                dialogue.text = currentNode.Text;
            }
            
        }

        private void HasDecision(Node currentNode,bool hasChoices, GameObject dialogueBox, bool isBox)
        {
            if (dialogueBox.transform.Find("Decide") == true)
            {
                
                var decision = dialogueBox.transform.Find("Decide");
                decision.gameObject.SetActive(hasChoices);
                if (hasChoices)
                {
                    for (var i = 0; i < decision.childCount; i++)
                    {
                        var currentChild = decision.GetChild(i);
                        currentChild.GetComponentInChildren<Text>().text = currentNode.choices[i].Text;
                        currentChild.GetComponent<Button>().onClick.AddListener(delegate { Choice(currentChild.gameObject, currentNode, isBox, dialogueBox); });
                    }
                    PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Disable();
                }
            }
        }
        
        
        private void Choice(GameObject currentChild, Node currentNode, bool isBox, GameObject currentParent)
        {
            
            switch (currentChild.gameObject.name)
            {
                case "Option 1":
                    NewMethod(0,currentChild, currentNode, isBox, currentParent);
                    break;
                case "Option 2":
                    NewMethod(1,currentChild, currentNode, isBox, currentParent);
                    break;
            }
        }

        private void NewMethod(int choice,GameObject currentChild, Node currentNode, bool isBox, GameObject currentParent)
        {
            var nextNode = DialogueManager.Instance.Nodes.Find(node => node.node_name == currentNode.choices[choice].next);
            if (isBox)
            {
                currentParent.SetActive(false);
            }
            else
            {
                currentChild.transform.parent.gameObject.SetActive(false);
            }
            InteractionHandler.Instance.OnUpdateNode(nextNode);
            PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Enable();
        }

        private void CloseDialogue()
        {
            foreach (var box in _dialogueBoxes.Where(box => box.activeSelf))
            {
                box.transform.Find("Dialogue").GetComponent<Text>().text = string.Empty;
                box.SetActive(false);
            }
        }
        
    }
}