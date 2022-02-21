using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    public bool loadMainMenu = false;

    [Header("Debug menus")]
    [SerializeField] private DebugMenuManager debugMenuManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnOpenScenesDebugMenu(InputAction.CallbackContext context)
    {
        debugMenuManager.OpenDebugMenu(DebugMenu.DebugMenuId.SCENES);
    }
}
