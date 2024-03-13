using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace Simpleverse
{
    public class DialogueTrigger : MonoBehaviour
    {
        public Message[] messages;
        public Actor[] actors;
        private DialogueManager dialogueManagerObj;

        public void StartDialogue()
        {
            dialogueManagerObj.OpenDialogue(messages, actors);
        }
        void Awake()
        {
            dialogueManagerObj = FindObjectOfType<DialogueManager>();

        }

    }

    [System.Serializable]
    public class Message
    {
        public int actorId;
        [TextArea]
        public string message;
    }

    [System.Serializable]
    public class Actor
    {
        public string name;
        public Sprite actorSpriteImage;
    }

}