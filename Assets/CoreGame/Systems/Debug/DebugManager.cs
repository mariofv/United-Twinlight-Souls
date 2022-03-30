using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    public bool loadMainMenu = false;
    public bool showEnemyDebugInfo = false;
    public bool godMode = false;

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

    public void OnOpenCharacterDebugMenu(InputAction.CallbackContext context)
    {
        debugMenuManager.OpenDebugMenu(DebugMenu.DebugMenuId.CHARACTERS);
    }

    public void OnOpenEnemyDebugMenu(InputAction.CallbackContext context)
    {
        debugMenuManager.OpenDebugMenu(DebugMenu.DebugMenuId.ENEMIES);
    }
}
