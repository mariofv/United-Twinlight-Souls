using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelAsset", menuName = "LevelAsset", order = 1)]
[System.Serializable]
public class LevelAsset : ScriptableObject
{
    public string levelName;
    public string levelScene;
    public AudioClip levelMusic;
}
