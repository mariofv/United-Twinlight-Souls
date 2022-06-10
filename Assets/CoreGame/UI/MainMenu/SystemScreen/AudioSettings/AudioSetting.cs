using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private void Awake()
    {
        leftButton.onClick.AddListener(DecreaseSetting);
        rightButton.onClick.AddListener(IncreaseSetting);
    }

    public void IncreaseSetting()
    {
        Debug.Log("Right pressed");
    }

    public void DecreaseSetting()
    {
        Debug.Log("Left pressed");
    }
}
