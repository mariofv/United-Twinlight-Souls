using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyObject : MonoBehaviour
{
    private Camera mainCamera;
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;
        objectRenderer.material.SetVector("_ObjScreenPos", screenPoint);
    }
}
