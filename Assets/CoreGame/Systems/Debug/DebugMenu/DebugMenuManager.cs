using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenuManager : MonoBehaviour
{
    [System.Serializable]
    public struct DebugMenuTuple
    {
        public DebugMenu.DebugMenuId debugMenuId;
        public DebugMenu debugMenu;
    }

    public List<DebugMenuTuple> debugMenus;

    void Update()
    {

    }

    public void OpenDebugMenu(DebugMenu.DebugMenuId debugMenuId)
    {
        foreach (DebugMenuTuple debugMenuTuple in debugMenus)
        {
            if (debugMenuTuple.debugMenuId == debugMenuId)
            {
                debugMenuTuple.debugMenu.enabled = !debugMenuTuple.debugMenu.enabled;
            }
            else
            {
                debugMenuTuple.debugMenu.enabled = false;
            }
        }
    }
}