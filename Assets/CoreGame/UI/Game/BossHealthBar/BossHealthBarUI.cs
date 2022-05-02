using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;
    private float originalWidth;

    void Awake()
    {
        originalWidth = fillTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InflictDamage(float currentHealthPercentage, float previousHealthPercentage)
    {
        float transformRight = (1 - currentHealthPercentage) * originalWidth;
        fillTransform.SetRight(transformRight);
    }
}
