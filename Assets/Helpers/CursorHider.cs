using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHider
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void HideCursor()
    {
        Cursor.visible = false;
    }

    public static void ShowCursor()
    {
        Cursor.visible = true;
    }
}
