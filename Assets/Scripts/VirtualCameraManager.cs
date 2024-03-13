using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

namespace Simpleverse
{
    public class VirtualCameraManager : MonoBehaviour
    {
        public void DisableCamera(GameObject camera)
        {
            camera.SetActive(false);
        }

        public void EnableCamera(GameObject camera)
        {
            camera.SetActive(true);
        }

        public void FirstPersonPOV(bool isEnabled)
        {
            SpatialBridge.cameraService.forceFirstPerson = isEnabled;
        }

        public void FocusOnNPC(GameObject camera, GameObject targetObj)
        {
            if (targetObj != null)
            {
                Transform targetsTransform = targetObj.transform;
                if (!camera.activeSelf)
                {
                    EnableCamera(camera);
                }

                // Position the camera slightly above and behind the NPC
                camera.transform.position = targetsTransform.position + new Vector3(0, 0.5f, -3f);


                // Make the camera always look at the NPC
                camera.transform.LookAt(targetsTransform);
            }
            else
            {
                Debug.Log("NPCVirtualCameraObj or NPC is null");
            }
        }

    }

}


