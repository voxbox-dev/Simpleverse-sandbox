using System.Collections;
using System.Collections.Generic;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace Simpleverse
{
    [RequireComponent(typeof(SpatialInteractable))]
    public class DialogueActor : MonoBehaviour
    {
        [SerializeField]
        private string speakerName;
        [SerializeField]
        private DialogueSO dialogue;
        private SpatialInteractable interactable;
        private DialogueManager2 dialogueManager;
        private VirtualCameraManager virtualCameraManager;
        private PlayerController playerController;
        [SerializeField]
        private float interactionDistance = 1f; // The distance the player should stand from the NPC when interacting
        void Start()
        {
            interactable = GetComponent<SpatialInteractable>();
            interactable.onInteractEvent += OnInteract;
            dialogueManager = FindAnyObjectByType<DialogueManager2>();
            virtualCameraManager = FindAnyObjectByType<VirtualCameraManager>();
            playerController = FindAnyObjectByType<PlayerController>();
        }

        public void OnInteract()
        {

            playerController.DisablePlayerMove(true); // disable movement
            FaceNPC();
            SpeakTo();

        }
        public void OnEndInteract()
        {

            playerController.DisablePlayerMove(false); // enable movement
            virtualCameraManager.DeactivateFirstPersonPOV();
        }
        void SpeakTo()
        {
            Vector3 dialoguePosition = transform.position + new Vector3(0, 1F, -0.61f); // Adjust the Y value as needed
            dialogueManager.SetDialoguePosition(dialoguePosition);
            dialogueManager.StartDialogue(dialogue, speakerName, dialogue.RootNodeID);
        }
        void OnDestroy()
        {
            interactable.onInteractEvent -= OnInteract;
        }
        void FaceNPC()
        {
            // Calculate the position in front of the NPC
            Vector3 targetPosition = transform.position - transform.forward * interactionDistance;
            // Move the player to the target position
            SpatialBridge.actorService.localActor.avatar.position = targetPosition;
            // Make the player face the NPC
            Vector3 lookPos = targetPosition - SpatialBridge.actorService.localActor.avatar.position;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            SpatialBridge.actorService.localActor.avatar.SetPositionRotation(targetPosition, rotation);
            virtualCameraManager.ActivateFirstPersonPOV();
            SpatialBridge.cameraService.ScreenToWorldPoint(lookPos);



        }


    }
}
