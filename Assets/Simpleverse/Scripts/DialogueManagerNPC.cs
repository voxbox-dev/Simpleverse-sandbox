using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using TMPro;

namespace Simpleverse
{

    public class DialogueManagerNPC : MonoBehaviour
    {
        //UI References
        [SerializeField]
        private GameObject dialogueCanvas;
        [SerializeField]
        private TMP_Text speakerNameText;
        [SerializeField]
        private TMP_Text dialogueText;
        [SerializeField]
        private Image speakerImage;
        [SerializeField]
        private GameObject virtualCameraManagerObject;
        [SerializeField]
        private GameObject playerControllerObject;
        [SerializeField]
        private GameObject npc;

        public void StartDialogue()
        {
            if (dialogueCanvas != null)
            {
                Debug.Log("BEFORE TOGGLE: CANVAS STATE: " + dialogueCanvas.activeSelf);
                // Toggle dialogue canvas
                dialogueCanvas.SetActive(!dialogueCanvas.activeSelf);
                ToggleSpatialGUI(dialogueCanvas.activeSelf);

                // Start NPC camera
                if (virtualCameraManagerObject != null && npc != null)
                {
                    virtualCameraManagerObject.GetComponent<VirtualCameraManager>().FocusOnNPC(npc.transform);
                }
                else
                {
                    Debug.Log("Virtual Camera Manager is null");
                }

                // Toggle player controller
                playerControllerObject.GetComponent<PlayerController>().TogglePlayerMove(npc, dialogueCanvas.activeSelf);
            }
            else
            {
                Debug.Log("Dialogue Canvas is null");
            }
        }
        private void ToggleSpatialGUI(bool isEnabled)
        {
            if (SpatialBridge.coreGUIService != null)
            {
                Debug.Log("CORE GUI: " + !isEnabled);
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