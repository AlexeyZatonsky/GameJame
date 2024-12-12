using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class DialogueLine
    {
        [TextArea(3, 10)]
        public string text;
        public float initialDelay = 0f;
        public float displayTime = 3f;
        public float delayBeforeNext = 0.5f;
    }

    public DialogueLine[] dialogueLines;
}
