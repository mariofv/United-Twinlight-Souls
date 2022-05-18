using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBubblesAnimation : MonoBehaviour
{
    [SerializeField] private float currentProgress;
    [SerializeField] private float bubbleSpeed;

    private Material bubbleMaterial;
    private float originalScale;

    private void Awake()
    {
        bubbleMaterial = GetComponent<MeshRenderer>().material;
        originalScale = transform.localScale.x;
        bubbleMaterial.SetTextureOffset("_NoiseTex", Vector2.one * currentProgress);
    }

    // Update is called once per frame
    void Update()
    {
        currentProgress = currentProgress - bubbleSpeed * Time.deltaTime;
        if (currentProgress < 0f)
        {
            currentProgress += 1f;
        }

        bubbleMaterial.SetFloat("_FadeProgress", 1f - currentProgress);
        transform.localScale = Vector3.one * originalScale * currentProgress;
    }
}
