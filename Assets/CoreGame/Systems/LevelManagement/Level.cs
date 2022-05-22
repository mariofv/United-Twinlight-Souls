using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public abstract class Level : MonoBehaviour
{
    public Cinematic introLevelCinematic;

    [Header("Special Transforms")]
    public Transform startPosition;
    public Transform voidPosition;

    public abstract CinemachineVirtualCamera GetCurrentCamera();

}
