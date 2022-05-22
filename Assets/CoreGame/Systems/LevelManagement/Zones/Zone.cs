using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Zone : MonoBehaviour
{
    public ZoneCamera zoneCamera;
    public Transform zoneCombatAreaTransform;
    public ZoneExitBarrier zoneExitBarrier;

    [Header("Purification")]
    [SerializeField] private float purificationTime;
    [SerializeField] private List<MeshRenderer> zoneMeshes;
    private List<Material> zoneMaterials;
    float currentTime = 0f;
    bool purifying = false;


    private void Awake()
    {
        zoneMaterials = new List<Material>();
        for (int i = 0; i < zoneMeshes.Count; ++i)
        {
            zoneMaterials.Add(zoneMeshes[i].material);
        }
    }

    private void Update()
    {
        if (purifying)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(currentTime / purificationTime, 1f);
            SetZonePurification(progress);

            if (progress == 1f)
            {
                purifying = false;
            }
        }
    }

    public void Purify()
    {
        currentTime = 0f;
        purifying = true;
    }

    public void SetZonePurification(float purification)
    {
        for (int i = 0; i < zoneMaterials.Count; ++i)
        {
            zoneMaterials[i].SetFloat("_FadeProgress", 1f - purification);
        }

    }
}
