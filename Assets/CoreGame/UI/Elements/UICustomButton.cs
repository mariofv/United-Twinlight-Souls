using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICustomButton : MonoBehaviour
{
    [SerializeField] private UISelectableSound buttonSelectableSoundManager;
    [SerializeField] private Image buttonBlur;
    [SerializeField] private GameObject buttonGameObject;
    [SerializeField] private float blurSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == buttonGameObject)
        {
            Color newColor = buttonBlur.color;
            newColor.a = (Mathf.Sin(Time.time * blurSpeed) + 1) * 0.25f;
            buttonBlur.color = newColor;
        }
        else
        {
            Color newColor = buttonBlur.color;
            newColor.a = 0f;
            buttonBlur.color = newColor;
        }
    }

    public void SelectWithoutSound()
    {
        buttonSelectableSoundManager.enabled = false;
        EventSystem.current.SetSelectedGameObject(buttonGameObject);
        buttonSelectableSoundManager.enabled = true;
    }
}
