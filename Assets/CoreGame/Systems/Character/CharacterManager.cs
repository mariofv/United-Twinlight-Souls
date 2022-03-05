using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterMovementManager characterMovementManager;
    public CharacterVisualsManager characterVisualsManager;

    // Start is called before the first frame update
    void Start()
    {
        characterMovementManager.Link(this);
        characterVisualsManager.Link(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector2 inputedMovement)
    {
        characterMovementManager.Move(inputedMovement);
        characterVisualsManager.SetMoving(characterMovementManager.IsMoving());
    }

    public void Jump()
    {
        characterMovementManager.Jump();
    }

    public void Teleport(Vector3 position)
    {
        characterMovementManager.Teleport(position);
    }

    public void Orientate(Quaternion orientation)
    {
        characterMovementManager.Orientate(orientation);
    }

    public void EnableMovement()
    {
        characterMovementManager.EnableMovement();
    }

    public void DisableMovement()
    {
        characterMovementManager.DisableMovement();
    }
}
