using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : Level
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public override CinemachineVirtualCamera GetCurrentCamera()
    {
        return virtualCamera;
    }
}
