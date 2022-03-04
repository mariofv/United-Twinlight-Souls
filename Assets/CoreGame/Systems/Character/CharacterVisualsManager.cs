using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualsManager : CharacterSubManager
{
    [SerializeField] private Animator characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        characterAnimator.SetBool("isAirborne", characterManager.characterMovementManager.IsAirbone());
        characterAnimator.SetFloat("verticalVelocity", characterManager.characterMovementManager.GetVerticalVelocity());
    }

    public void SetMoving(bool moving)
    {
        characterAnimator.SetBool("moving", moving);
    }
}
