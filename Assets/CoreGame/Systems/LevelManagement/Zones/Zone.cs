using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Zone : MonoBehaviour
{
    public ZoneCamera zoneCamera;
    public Transform zoneCombatAreaTransform;
    public ZoneExitBarrier zoneExitBarrier;
    public Cinematic zonePurificationCinematic;

    [Header("Purification")]
    [SerializeField] private List<MeshRenderer> zoneMeshes;
    private List<Material> zoneMaterials;


    private void Awake()
    {
        zoneMaterials = new List<Material>();
        for (int i = 0; i < zoneMeshes.Count; ++i)
        {
            zoneMaterials.Add(zoneMeshes[i].material);
        }
    }

    public void SetZonePurification(float purification)
    {
        for (int i = 0; i < zoneMaterials.Count; ++i)
        {
            zoneMaterials[i].SetFloat("_FadeProgress", 1f - purification);
        }

    }
}
