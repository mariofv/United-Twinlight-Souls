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
        RELEASING
    }

    [SerializeField] private Renderer shieldRenderer;
    [SerializeField] private SphereCollider shieldCollider;

    [Header("Hit")]
    [SerializeField] private AnimationCurve displacementCurve;
    [SerializeField] private float displacementMagnitude;
    [SerializeField] private float lerpSpeed;
    private bool hitAnimation = false;
    private float hitLerp = 0f;

    [Header("Casting")]
    [SerializeField] private float raiseSpeed;
    [SerializeField] private float releaseSpeed;

    private ShieldState currentState;

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

    public void HitShield(Vector3 hitPos)
    {
        shieldRenderer.material.SetVector("_HitPos", hitPos);
        StopAllCoroutines();
        hitLerp = 0f;
        hitAnimation = true;
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
}