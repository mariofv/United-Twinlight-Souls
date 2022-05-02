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

    public void InflictDamage(int healthAfterDamage, int healthBeforeDamage, int maxHealth)
    {
        float healthProgress = ((float)healthAfterDamage) / maxHealth;
        float transformRight = (1 - healthProgress) * originalWidth;
        fillTransform.SetRight(transformRight);
    }
}
