using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Setting : MonoBehaviour
{
    [SerializeField] protected Button leftButton;
    [SerializeField] protected Button rightButton;

    protected virtual void Awake()
    {
        leftButton.onClick.AddListener(DecreaseSetting);
        rightButton.onClick.AddListener(IncreaseSetting);
    }

    public abstract void IncreaseSetting();
    public abstract void DecreaseSetting();

}
