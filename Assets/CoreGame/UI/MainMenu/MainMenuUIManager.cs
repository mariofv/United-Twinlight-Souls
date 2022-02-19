using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private List<Transform> cameraTransforms;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.cameraManager.mainCamera.transform.position = cameraTransforms[0].position;
        GameManager.instance.cameraManager.mainCamera.transform.rotation = cameraTransforms[0].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
