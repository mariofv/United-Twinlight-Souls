using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugSave", menuName = "DebugSave", order = 1)]
[System.Serializable]
public class DebugSave : ScriptableObject
{
    public SaveData debugSaveData;
}
