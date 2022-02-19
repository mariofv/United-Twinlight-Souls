using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
[System.Serializable]
public class Level : ScriptableObject
{
    public string levelName;
    public string levelScene;
}
