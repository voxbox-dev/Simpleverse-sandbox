using System.Collections;
using System.Collections.Generic;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace Simpleverse
{

    public class DialogueActor : MonoBehaviour
    {
        [SerializeField]
        private string speakerName;
        [SerializeField]
        private DialogueSO dialogue;
        [SerializeField]
        private float interactionDistance = 1f; // The distance from the NPC when interacting

        [SerializeField]
        private SpatialInteractable interactableStart;
        [SerializeField]
        private GameObject interactableContinue;
        private DialogueManager dialogueManager;
        private VirtualCameraManager virtualCameraManager;
        private PlayerController playerController;
        private bool hasInteractionStarted;
        private int currNodeID;
        void Start()
        {
            interactableContinue.SetActive(false);
            interactableStart.gameObject.SetActive(true);
            interactableStart.onInteractEvent += OnInteract;
            interactableContinue.GetComponent<SpatialInteractable>().onInteractEvent += OnInteract;
            dialogueManager = FindAnyObjectByType<DialogueManager>();
            virtualCameraManager = FindAnyObjectByType<VirtualCameraManager>();
            playerController = FindAnyObjectByType<PlayerController>();
            hasInteractionStarted = false;
        }
        void Update()
        {
            // Rotate the NPC to face the player
            transform.LookAt(SpatialBridge.actorService.localActor.avatar.position);
        }

        public void OnInteract()
        {
            if (hasInteractionStarted == false)
            {
                // On first interaction...
                playerController.DisablePlayerMove(true); // disable movement
                virtualCameraManager.ActivateFirstPersonPOV();
                dialogueManager.SetDialoguePosition(transform.position);
                currNodeID = dialogue.RootNodeID;
                interactableStart.gameObject.SetActive(false);
                interactableContinue.SetActive(true);
                hasInteractionStarted = true;
            }
            SpeakTo(currNodeID);
            currNodeID++;

        }
        void OnEndInteract()
        {
            dialogueManager.EndDialogue();
            hasInteractionStarted = false;
            interactableStart.gameObject.SetActive(true);
            interactableContinue.SetActive(false);
            playerController.DisablePlayerMove(false); // enable movement
            virtualCameraManager.DeactivateFirstPersonPOV();
        }


        void SpeakTo(int currNodeID)
        {
            if (dialogue.GetNodeByID(currNodeID) == null)
            {
                OnEndInteract();

            }
            else
            {
                dialogueManager.StartDialogue(dialogue, speakerName, currNodeID);

            }
        }
        void OnDestroy()
        {
            interactableStart.onInteractEvent -= OnInteract;
            interactableContinue.GetComponent<SpatialInteractable>().onInteractEvent -= OnInteract;
        }
    }
}
