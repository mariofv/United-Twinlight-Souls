using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLine : MonoBehaviour
{
    [SerializeField] private Transform pointsParent;
    public CinemachineSmoothPath cinemachinePath;

    private void Awake()
    {
        cinemachinePath.m_Waypoints = new CinemachineSmoothPath.Waypoint[pointsParent.childCount];
        for (int i = 0; i < pointsParent.childCount; ++i)
        {
            cinemachinePath.m_Waypoints[i].position = pointsParent.GetChild(i).transform.position;
        }
    }

    public float GetProgress(Vector3 position)
    {
        return cinemachinePath.FindClosestPoint(position, 0, -1, 5);
    }

    public Vector3 GetPoint(float progress)
    {
        return cinemachinePath.EvaluatePositionAtUnit(progress, CinemachinePathBase.PositionUnits.Normalized);
    }
}
