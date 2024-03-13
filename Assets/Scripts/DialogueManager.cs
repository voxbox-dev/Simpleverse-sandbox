using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Simpleverse
{
    public class DialogueManager : MonoBehaviour
    {
        public Image actorImageObj;
        public TMP_Text actorName;
        public TMP_Text messageText;
        public RectTransform backgroundBox;
        private Message[] messagesArr;
        private Actor[] actorsArr;
        private int activeMessageIndx;
        public static bool isActive = false;
        private PlayerController playerControllerObj;
        private VirtualCameraManager cameraManagerObj;
        private GameObject triggerObj;
        [SerializeField]
        private GameObject cameraNPC;
        [SerializeField]
        private GameObject dialoguePanel;

        void Start()
        {
            playerControllerObj = FindAnyObjectByType<PlayerController>();
            cameraManagerObj = FindAnyObjectByType<VirtualCameraManager>();
            triggerObj = FindAnyObjectByType<DialogueTrigger>().gameObject;
            if (dialoguePanel.activeSelf == true)
            {
                // Ensures the dialogue default state is hiding
                dialoguePanel.SetActive(false);
            }
        }

        public void OpenDialogue(Message[] messages, Actor[] actors)
        {
            if (!isActive)
            {
                // Set Dialogue content
                messagesArr = messages;
                actorsArr = actors;
                activeMessageIndx = 0;
                isActive = true;

                // Show Dialogue panel
                dialoguePanel.SetActive(true);

                // Hide Spatial UI
                ToggleSpatialGUI(isActive);
                // Disable movement
                playerControllerObj.RotateAvatarTowards(triggerObj);
                playerControllerObj.TogglePlayerMove(isActive);
                // Focus camera on NPC
                cameraManagerObj.FocusOnNPC(cameraNPC, triggerObj);
                DisplayMessage();
            }

        }

        void DisplayMessage()
        {
            // Get current actor and message
            Message currentMessage = messagesArr[activeMessageIndx];
            Actor currentActor = actorsArr[currentMessage.actorId];
            // Update dialogue box
            actorName.SetText(currentActor.name);
            actorImageObj.sprite = currentActor.actorSpriteImage;
            messageText.SetText(currentMessage.message);
        }

        public void NextMessage()
        {
            activeMessageIndx++;
            if (activeMessageIndx < messagesArr.Length)
            {
                DisplayMessage();
            }
            else
            {
                ResetState();
                isActive = false;
            }
        }

        private void ResetState()
        {
            // Update dialogue box
            actorName.SetText("");
            actorImageObj.sprite = null;
            messageText.SetText("");

            // Reset Camera
            cameraManagerObj.DisableCamera(cameraNPC);
            // Hide Dialogue
            dialoguePanel.SetActive(false);
            // Show SpatialUI
            ToggleSpatialGUI(false);

            // Re-enable player movement
            playerControllerObj.TogglePlayerMove(false);
        }

        private void ToggleSpatialGUI(bool isEnabled)
        {
            if (SpatialBridge.coreGUIService != null)
            {
                // Disable the chat. Hiding it and removing the ability to re-open it
                SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.All, enabled: !isEnabled); // false
            }
            else
            {
                Debug.Log("SpatialBridge.coreGUIService is null");
            }
        }
    }
}
