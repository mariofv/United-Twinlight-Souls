using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualsManager : CharacterSubManager
{
    [SerializeField] private Animator characterAnimator;
    
    [Header("Material")]
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private float transitionTime;
    private Material originalMaterial;
    private float currentTime = 0f;
    private bool showing = false;
    private bool hiding = false;

    // Start is called before the first frame update
    void Awake()
    {
        originalMaterial = characterRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterManager.IsMovementEnabled())
        {
            characterAnimator.SetBool("isAirborne", characterManager.characterMovementManager.IsAirbone());
            characterAnimator.SetFloat("verticalVelocity", characterManager.characterMovementManager.GetVerticalVelocity());
        }
        if (showing)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(currentTime / transitionTime, 1f);

            Color newColor = new Color(1f, 1f, 1f, progress);
            characterRenderer.material.SetColor("_Color", newColor);

            if (progress == 1f)
            {
                showing = false;
                characterRenderer.material = originalMaterial;
            }
        }
        if (hiding)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Min(currentTime / transitionTime, 1f);

            Color newColor = new Color(1f, 1f, 1f, 1f - progress);
            characterRenderer.material.SetColor("_Color", newColor);
            
            if (progress == 1f)
            {
                hiding = false;
            }
        }

    }

    public void SetMoving(bool moving)
    {
        characterAnimator.SetBool("moving", moving);
    }

    public void TriggerLightAttack()
    {
        characterAnimator.SetTrigger("attack");
    }

    public void StartStun()
    {
        characterAnimator.SetTrigger("startStun");
    }

    public void EndStun()
    {
        characterAnimator.SetTrigger("endStun");
    }

    public void TriggerDeath()
    {
        characterAnimator.SetTrigger("death");
    }

    public void TriggerRevive()
    {
        characterAnimator.SetTrigger("revive");
    }

    public float GetCurrentAnimationProgress()
    {
        AnimatorStateInfo currentAnimatorState = characterAnimator.GetCurrentAnimatorStateInfo(0);
        float currentAnimationProgress = currentAnimatorState.normalizedTime % 1;

        return currentAnimationProgress;
    }

    public void ResetVisuals()
    {
        characterAnimator.SetBool("isAirborne", false);
        characterAnimator.SetBool("moving", false);
        characterAnimator.SetFloat("verticalVelocity", 0f);
    }

    public void ShowMesh()
    {
        showing = true;
        hiding = false;
        currentTime = 0f;
    }

    public void HideMesh()
    {
        showing = false;
        hiding = true;
        currentTime = 0f;
        characterRenderer.material = transparentMaterial;
    }
}
