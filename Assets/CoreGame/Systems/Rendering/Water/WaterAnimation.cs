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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentTiling = animatedMaterial.GetTextureOffset("_MainTex");
        currentTiling += Time.deltaTime * animationSpeed;
        animatedMaterial.SetTextureOffset("_MainTex", currentTiling);
        animatedMaterial.SetTextureOffset("_FadeTex", currentTiling);
    }
}
