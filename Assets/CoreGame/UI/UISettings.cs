using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    public static UISettings instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            gameUISettings = new GameUISettings();
        }
    }

    public class MainMenuUISettings
    {
        public const float LOGO_DISPLAY_TIME = 2.5f;
    };

    public class GameUISettings
    {
        public const float DISPLAY_TIME = 0.125f;
        public const float DISPLAY_OFFSET = 100;

        public const float LOADING_SCREEN_TIP_DISPLAY_TIME = 5;
        //public static TweenParams TWEEN_PARAMS = new TweenParams().SetAutoKill(false).SetUpdate(true);
    };

    public GameUISettings gameUISettings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
