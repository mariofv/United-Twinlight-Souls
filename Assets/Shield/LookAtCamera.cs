using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookAtRotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position);
        lookAtRotation *= Quaternion.Euler(0, 90, 0);
        transform.rotation = lookAtRotation;   
    }
}
