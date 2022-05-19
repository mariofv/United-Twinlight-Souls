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

    [SerializeField] private AnimationCurve displacementCurve;
    [SerializeField] private float displacementMagnitude;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float raiseSpeed;

    private ShieldState currentState;
    private float currentShieldProgress = 0f;
    private Renderer shieldRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
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

                if (raisingProgress == 1f)
                {
                    currentState = ShieldState.RAISED;
                }
                break;

            case ShieldState.RAISED:
                break;

            case ShieldState.RELEASING:
                float releasingProgress = shieldRenderer.material.GetFloat("_Disolve");

                releasingProgress = Mathf.Min(Time.deltaTime * raiseSpeed + releasingProgress, 1f);
                shieldRenderer.material.SetFloat("_Disolve", releasingProgress);

                if (releasingProgress == 0f)
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
        StartCoroutine(Coroutine_HitDisplacement());
    }

    public void Raise()
    {
        currentState = ShieldState.RAISING;
    }

    public void Release()
    {
        currentState = ShieldState.RELEASING;
    }

    IEnumerator Coroutine_HitDisplacement()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            shieldRenderer.material.SetFloat("_DisplacementStrength", displacementCurve.Evaluate(lerp) * displacementMagnitude);
            lerp += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }
}