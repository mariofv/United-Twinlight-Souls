using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private AnimationCurve displacementCurve;
    [SerializeField] private float displacementMagnitude;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float disolveSpeed;

    private bool shieldOn;
    private Renderer shieldRenderer;
    private Coroutine disolveCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
    }

    public void HitShield(Vector3 hitPos)
    {
        shieldRenderer.material.SetVector("_HitPos", hitPos);
        StopAllCoroutines();
        StartCoroutine(Coroutine_HitDisplacement());
    }

    public void OpenCloseShield()
    {
        float target = 1;
        if (shieldOn)
        {
            target = 0;
        }
        shieldOn = !shieldOn;
        if (disolveCoroutine != null)
        {
            StopCoroutine(disolveCoroutine);
        }
        disolveCoroutine = StartCoroutine(Coroutine_DisolveShield(target));
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

    IEnumerator Coroutine_DisolveShield(float target)
    {
        float start = shieldRenderer.material.GetFloat("_Disolve");
        float lerp = 0;
        while (lerp < 1)
        {
            shieldRenderer.material.SetFloat("_Disolve", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * disolveSpeed;
            yield return null;
        }
    }
}