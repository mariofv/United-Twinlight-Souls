using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public CameraRail cameraRail;

    private void Awake()
    {
        GameManager.instance.cameraManager.SetCurrentCameraRail(cameraRail);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
