using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private enum ShieldState
    {
        DISABLED,
        RAISING,
        RAISED,
        RELEASING,
        BROKEN
    }

    [Header("Shield components")]
    [SerializeField] private Renderer shieldRenderer;
    [SerializeField] private SphereCollider shieldCollider;
    private Color originalShieldColor;
    private Color brokenShieldColor;

    [Header("Shield stats")]
    [SerializeField] private int maxHealth;
    private int currentHealth = 0;
    [SerializeField] private float healthRegenerationSpeed;
    [SerializeField] private float timeUntilHealthRegeneration;

    [Header("Hit")]
    [SerializeField] private AnimationCurve displacementCurve;
    [SerializeField] private float displacementMagnitude;
    [SerializeField] private float lerpSpeed;
    private bool hitAnimation = false;
    private float hitLerp = 0f;

    [Header("Casting")]
    [SerializeField] private float raiseSpeed;
    [SerializeField] private float releaseSpeed;

    [Header("Breaking")]
    [SerializeField] private BrokenShield brokenShield;

    private ShieldState currentState;

    private void Awake()
    {
        originalShieldColor = shieldRenderer.material.GetColor("_FresnelColor");
        brokenShieldColor = brokenShield.GetBrokenShieldColor();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case ShieldState.DISABLED:
                break;
            case ShieldState.RAISING:
                float raisingProgress = shieldRenderer.material.GetFloat("_Disolve");

                raisingProgress = Mathf.Max(raisingProgress - Time.deltaTime * raiseSpeed, 0f);
                shieldRenderer.material.SetFloat("_Disolve", raisingProgress);

                if (raisingProgress == 0f)
                {
                    currentState = ShieldState.RAISED;
                    shieldCollider.enabled = true;
                }
                break;

            case ShieldState.RAISED:
                if (hitAnimation)
                {
                    hitLerp = Mathf.Min(1f, hitLerp + Time.deltaTime * lerpSpeed);
                    shieldRenderer.material.SetFloat("_DisplacementStrength", displacementCurve.Evaluate(hitLerp) * displacementMagnitude);

                    if (hitLerp == 1f)
                    {
                        hitAnimation = false;
                    }
                }
                break;

            case ShieldState.RELEASING:
                float releasingProgress = shieldRenderer.material.GetFloat("_Disolve");

                releasingProgress = Mathf.Min(Time.deltaTime * releaseSpeed + releasingProgress, 1f);
                shieldRenderer.material.SetFloat("_Disolve", releasingProgress);

                if (releasingProgress == 1f)
                {
                    currentState = ShieldState.DISABLED;
                }
                break;
        }
    }

    public void HitShield(int damage, Vector3 hitPos)
    {
        shieldRenderer.material.SetVector("_HitPos", hitPos);
        hitLerp = 0f;
        hitAnimation = true;

        int newHealth = Mathf.Max(0, currentHealth - damage);
        SetShieldHealth(newHealth);
    }

    public void Raise()
    {
        currentState = ShieldState.RAISING;
    }

    public void Release()
    {
        currentState = ShieldState.RELEASING;
        shieldCollider.enabled = false;
    }

    private void SetShieldHealth(int health)
    {
        currentHealth = health;
        float currentHealthProgress = (float)currentHealth / maxHealth;
        Color newShieldColor = Color.Lerp(originalShieldColor, brokenShieldColor, (1f - currentHealthProgress));
        shieldRenderer.material.SetColor("_FresnelColor", newShieldColor);
        shieldRenderer.material.SetFloat("_BreakProgress", (1f - currentHealthProgress));

        if (currentHealth == 0)
        {
            BreakShield();
        }
    }

    private void BreakShield()
    {
        brokenShield.transform.position = transform.position;
        brokenShield.Explode();
    }

    public bool IsRaised()
    {
        return currentState == ShieldState.RAISED;
    }
}