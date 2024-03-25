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
        private Message[] messagesArr;
        private Actor[] actorsArr;
        private int activeMessageIndx;
        public static bool isActive = false;
        private PlayerController playerControllerObj;
        private VirtualCameraManager cameraManagerObj;
        [SerializeField]
        private GameObject cameraNPC;
        [SerializeField]
        private GameObject dialoguePanel;
        [SerializeField]
        private Button dialogueActionBtn;

        void Start()
        {
            playerControllerObj = FindAnyObjectByType<PlayerController>();
            cameraManagerObj = FindAnyObjectByType<VirtualCameraManager>();

            if (dialoguePanel.activeSelf == true)
            {
                // Ensures the dialogue default state is hiding
                dialoguePanel.SetActive(false);
            }
        }

        public void OpenDialogue(Message[] messages, Actor[] actors, GameObject triggerObj)
        {
            if (!isActive)
            {
                // Set Dialogue content
                messagesArr = messages;
                actorsArr = actors;
                activeMessageIndx = 0;
                isActive = true;

                // attach action to the dialogue button
                dialogueActionBtn.onClick.AddListener(NextMessage);

                // Show Dialogue panel
                dialoguePanel.SetActive(true);

                // Hide Spatial UI
                ToggleSpatialGUI(isActive);
                // Disable movement
                playerControllerObj.RotateAvatarTowards(triggerObj);
                playerControllerObj.DisablePlayerMove(isActive);
                // Focus camera on NPC
                cameraManagerObj.FocusOnNPC(triggerObj);
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
            cameraManagerObj.DisableCamera();
            // Hide Dialogue
            dialoguePanel.SetActive(false);
            // Show SpatialUI
            ToggleSpatialGUI(false);

            // Re-enable player movement
            playerControllerObj.DisablePlayerMove(false);

            // Remove click listener from button
            dialogueActionBtn.onClick.RemoveAllListeners();
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
