using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneExitBarrier : MonoBehaviour
{
    [SerializeField] private MeshCollider barrierCollider;
    [SerializeField] private float openingTime;
    [SerializeField] private float downDistance;
    private float currentTime = 0f;
    private bool opening = false;
    private float originalPosition;

    private void Awake()
    {
        originalPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(1f, currentTime / openingTime);

            Vector3 position = transform.position;
            position.y = Mathf.Lerp(originalPosition, originalPosition - downDistance, progress);
            transform.position = position;

            if (progress == 1f)
            {
                opening = false;
                barrierCollider.enabled = false;
            }
        }
    }

    public void Open()
    {
        opening = true;
        currentTime = 0f;
    }
}
