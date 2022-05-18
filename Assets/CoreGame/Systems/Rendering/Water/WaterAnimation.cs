using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    private Material animatedMaterial;
    [SerializeField] private Vector2 animationSpeed;

    private void Awake()
    {
        animatedMaterial = GetComponent<MeshRenderer>().material;
        animatedMaterial.SetVector("_Speed", new Vector4(animationSpeed.x, animationSpeed.y, 0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
