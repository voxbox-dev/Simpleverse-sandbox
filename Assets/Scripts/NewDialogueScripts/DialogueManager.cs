using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Simpleverse
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        public GameObject dialogueBoxParent;
        public TMP_Text speaker, message;

        public void StartDialogue(DialogueSO dialogue, string name, int currNodeID)
        {
            ShowDialogue();

            DialogueNode node = dialogue.GetNodeByID(currNodeID);
            speaker.text = name;
            message.text = node.dialogueText;
        }

        public void EndDialogue()
        {
            HideDialogue();
        }

        public void HideDialogue()
        {
            dialogueBoxParent.SetActive(false);

        }
        public void ShowDialogue()
        {
            dialogueBoxParent.SetActive(true);
        }
        public void SetDialoguePosition(Vector3 targetOpbjPosition)
        {
            Vector3 dialoguePosition = targetOpbjPosition + new Vector3(0, 1F, -0.61f); // Adjust the Y value as needed
            dialogueBoxParent.transform.position = dialoguePosition;
        }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // Initially hide dialogue
            HideDialogue();
        }


    }
}
