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

    public void TeleportPlayer(Vector3 position)
    {
        characterMovementManager.TeleportPlayer(position);
    }

    public void OrientatePlayer(Quaternion orientation)
    {
        characterMovementManager.OrientatePlayer(orientation);
    }
}
