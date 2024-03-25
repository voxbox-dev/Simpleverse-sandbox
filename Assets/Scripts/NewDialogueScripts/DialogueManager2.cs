using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SpatialSys.UnitySDK;

namespace Simpleverse
{
    public class DialogueManager2 : MonoBehaviour
    {
        public static DialogueManager2 Instance { get; private set; }
        public GameObject dialogueBoxParent;
        public TMP_Text speaker, message;
        public GameObject actionButtonPrefab;
        public Transform actionButtonContainer;
        private DialogueActor dialogueActorScript;


        void Start()
        {
            dialogueActorScript = FindAnyObjectByType<DialogueActor>();
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

        public void StartDialogue(DialogueSO dialogue, string name, int startNodeID)
        {
            ShowDialogue();

            DialogueNode node = dialogue.GetNodeByID(startNodeID);
            speaker.text = name;
            message.text = node.dialogueText;

            foreach (Transform child in actionButtonContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueResponse response in node.responses)
            {
                GameObject buttonObj = Instantiate(actionButtonPrefab, actionButtonContainer);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

                buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(dialogue, name, response));
            }
        }

        public void SelectResponse(DialogueSO currDialogue, string title, DialogueResponse currResponse)
        {
            if (currDialogue.GetNodeByID(currResponse.nextNodeID) != null)
            {
                StartDialogue(currDialogue, title, currResponse.nextNodeID);
            }
            else
            {
                EndDialogue();

            }
        }

        public void EndDialogue()
        {
            dialogueActorScript.OnEndInteract();
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
        public void SetDialoguePosition(Vector3 position)
        {
            dialogueBoxParent.transform.position = position;
        }


    }
}
