using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAsset", menuName = "DialogueAsset", order = 1)]
[System.Serializable]
public class DialogueAsset : ScriptableObject
{
    public List<string> dialogueMessages;
}
