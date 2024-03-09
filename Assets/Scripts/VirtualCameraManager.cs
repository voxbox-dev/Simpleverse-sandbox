using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

namespace Simpleverse
{
    public class VirtualCameraManager : MonoBehaviour
    {
        //         [SerializeField]
        //         private GameObject npcCamera;
        // 
        //         public void StartNPCCamera()
        //         {
        //             if (SpatialBridge.cameraService != null)
        //             {
        //                 Debug.Log("StartNPCCamera:");
        //                 // Toggle camera on/off
        //                 npcCamera.SetActive(!npcCamera.activeSelf);
        //             }
        //             else
        //             {
        //                 Debug.Log("SpatialBridge.cameraService is null");
        //             }
        //         }

        [SerializeField]
        private GameObject NPCVirtualCameraObj;

        public void FocusOnNPC(Transform NPC)
        {
            bool isCameraActive = NPCVirtualCameraObj.activeSelf;

            if (NPCVirtualCameraObj != null && NPC != null)
            {
                if (!isCameraActive)
                {
                    Debug.Log("NPCVirtualCameraObj is not active");
                    NPCVirtualCameraObj.transform.position = NPC.position + new Vector3(0, 0.5f, -3); // Position the camera slightly above and behind the NPC
                    NPCVirtualCameraObj.transform.LookAt(NPC); // Make the camera always look at the NPC
                    NPCVirtualCameraObj.SetActive(true);
                }
                else
                {
                    NPCVirtualCameraObj.SetActive(false);
                    Debug.Log("NPCVirtualCameraObj is already active");
                }
            }
            else
            {
                Debug.Log("NPCVirtualCameraObj or NPC is null");
            }
        }

    }
}


