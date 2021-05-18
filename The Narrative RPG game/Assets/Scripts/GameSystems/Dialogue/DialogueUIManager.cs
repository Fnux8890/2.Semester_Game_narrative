using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private GameObject PreviousDialogueBox;

        private void Awake()
        {
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                _dialogueBoxes.Add(gameObject.transform.GetChild(i).gameObject);
            }
            DialogueUIHandler.Instance.ExitDialogue += CloseDialogue;
            DialogueUIHandler.Instance.ShowDialogue += DisplayDialogue;
        }

        private void DisplayDialogue(Node currentNode)
        {
            BranchedDialogueOrNot(currentNode, currentNode.choices != null);
        }

        private void BranchedDialogueOrNot(Node currentNode, bool hasChoices)
        {
           StartCoroutine(HandleBox(currentNode,currentNode.is_box,hasChoices));
        }

        private IEnumerator HandleBox(Node currentNode, bool isBox, bool hasChoices)
        {
            if (PreviousDialogueBox != null)
            {
                PreviousDialogueBox.transform.Find("Continue").gameObject.SetActive(false);
            }
            if (PreviousDialogueBox != null && isBox)
            {
                _dialogueBoxes.Find(box => box.name == "DialogueBoxLeft").gameObject.SetActive(false);
                _dialogueBoxes.Find(box => box.name == "DialogueBoxRight").gameObject.SetActive(false);
            }

            if (PreviousDialogueBox != null && PreviousDialogueBox.name == "DialogueBox Top")
            {
                _dialogueBoxes.Find(box => box.name == "DialogueBox Top").gameObject.SetActive(false);
            }

            switch (isBox)
            {
                case true:
                {
                    var dialogueBox = _dialogueBoxes.Find(box => box.name == "DialogueBox Top");
                    PreviousDialogueBox = dialogueBox;
                    dialogueBox.gameObject.SetActive(true);
                    TurnDecisionPanelOff(dialogueBox);
                    yield return StartCoroutine(TypeWriter(currentNode.Text, dialogueBox, currentNode, hasChoices));
                    HasDecision(currentNode, hasChoices, dialogueBox, isBox);
                    break;
                }
                case false:
                {
                    GameObject dialogueBox;
                    if (currentNode.character == "Player")
                    {
                        dialogueBox = _dialogueBoxes.Find(box => box.name == "DialogueBoxLeft");
                        dialogueBox.transform.Find("Name").GetChild(0).GetComponent<Text>().text = currentNode.character;
                    }
                    else
                    {
                        dialogueBox = _dialogueBoxes.Find(box => box.name == "DialogueBoxRight");
                        dialogueBox.transform.Find("Name").GetChild(0).GetComponent<Text>().text = currentNode.character;
                    }
                    PreviousDialogueBox = dialogueBox;
                    dialogueBox.gameObject.SetActive(true);
                    TurnDecisionPanelOff(dialogueBox);
                    yield return StartCoroutine(TypeWriter(currentNode.Text, dialogueBox, currentNode, hasChoices));
                    HasDecision(currentNode,hasChoices, dialogueBox, isBox);
                    break;
                }
            }
        }

        private void HasDecision(Node currentNode,bool hasChoices, GameObject dialogueBox, bool isBox)
        {
            if (dialogueBox.transform.Find("Decision Panel") == true)
            {
                dialogueBox = dialogueBox.transform.Find("Decision Panel").gameObject;
                dialogueBox.gameObject.SetActive(false);
            }
            if (dialogueBox.transform.Find("Decide") == true)
            {
                
                var decision = dialogueBox.transform.Find("Decide");
                decision.gameObject.SetActive(hasChoices);
                if (hasChoices)
                {
                    dialogueBox.gameObject.SetActive(true);
                    for (var i = 0; i < decision.childCount; i++)
                    { 
                        var currentChild = decision.GetChild(i);
                        var textButtonText = currentChild.GetComponentInChildren<Text>();
                        textButtonText.text = currentNode.choices[i].Text ;
                        var rect = currentChild.GetComponent<RectTransform>().rect;
                        rect.width = textButtonText.gameObject.GetComponent<RectTransform>().rect.width;
                        currentChild.GetComponent<Button>().onClick.AddListener(delegate { Choice(currentChild.gameObject, currentNode, isBox, dialogueBox); });
                    }
                    PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Disable();
                }
            }
        }

        private void TurnDecisionPanelOff(GameObject dialogueBox)
        {
            if (dialogueBox.transform.Find("Decision Panel") == true)
            {
                var decisionPanel = dialogueBox.transform.Find("Decision Panel").gameObject;
                decisionPanel.gameObject.SetActive(false);
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
                currentParent.transform.parent.gameObject.SetActive(false);
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
                box.transform.Find("Dialogue").GetChild(0).GetComponent<Text>().text = string.Empty;
                box.SetActive(false);
            }
        }

        private IEnumerator TypeWriter(string text, GameObject objectToSet, Node currentNode, bool hasChoices)
        {
            objectToSet.transform.transform.Find("Continue").gameObject.SetActive(false);
            var textObject = objectToSet.transform.Find("Dialogue").GetChild(0).GetComponent<Text>();
            PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Disable();
            var sb = new StringBuilder();
            textObject.text = sb.ToString();
            foreach (var ch in text.ToCharArray())
            {
                textObject.text += ch;
                yield return new WaitForSeconds(1f/30);
            }
            yield return new WaitForSeconds(0.5f);
            objectToSet.transform.transform.Find("Continue").gameObject.SetActive(currentNode.choices == null);
            if (!hasChoices)
            {
                PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.Enable();
            }
        }
        
        
    }
}