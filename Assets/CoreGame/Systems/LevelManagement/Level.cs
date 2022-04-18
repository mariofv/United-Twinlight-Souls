using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class Level : MonoBehaviour
{
    [Header("Special Transforms")]
    public Transform startPosition;
    public Transform voidPosition;

    public abstract CinemachineVirtualCamera GetCurrentCamera();

}
