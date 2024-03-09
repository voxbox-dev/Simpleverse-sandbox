using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

namespace Simpleverse
{
    public class PlayerController : MonoBehaviour, IAvatarInputActionsListener
    {
        // VARIABLES
        private bool isMoveDisabled = false;
        // CUSTOM METHODS
        public void TogglePlayerMove(GameObject npc, bool disableMovement)
        {
            isMoveDisabled = disableMovement;

            if (isMoveDisabled)
            {
                Debug.Log("Disable Movement");
                SpatialBridge.inputService.StartAvatarInputCapture(isMoveDisabled, isMoveDisabled, isMoveDisabled, false, this);
                OnInputCaptureStarted(InputCaptureType.Avatar);
                StopAvatarMovement(npc);
            }
            else
            {
                Debug.Log("Enable Movement");
                SpatialBridge.inputService.ReleaseInputCapture(this);
                OnInputCaptureStopped(InputCaptureType.Avatar);
            }
        }
        private void StopAvatarMovement(GameObject npc)
        {
            // stop avatar movement 
            SpatialBridge.actorService.localActor.avatar.Move(Vector3.zero);
            // set rotation to face npc or other object
            Vector3 lookPos = npc.transform.position - SpatialBridge.actorService.localActor.avatar.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            SpatialBridge.actorService.localActor.avatar.SetPositionRotation(SpatialBridge.actorService.localActor.avatar.position, rotation);
        }

        // INTERFACE METHODS
        public void OnAvatarActionInput(InputPhase inputPhase)
        {

        }

        public void OnAvatarAutoSprintToggled(bool on)
        {

        }

        public void OnAvatarJumpInput(InputPhase inputPhase)
        {

        }

        public void OnAvatarMoveInput(InputPhase inputPhase, Vector2 inputMove)
        {

        }

        public void OnAvatarSprintInput(InputPhase inputPhase)
        {

        }

        public void OnInputCaptureStarted(InputCaptureType type)
        {
            Debug.Log("Input Capture Started");

        }

        public void OnInputCaptureStopped(InputCaptureType type)
        {
            Debug.Log("Input Capture Stopped");
        }


    }
}
