using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : CharacterSubManager
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpSpeed;

    private Vector3 movementVector;
    private float verticalVelocity = 0f;
    private bool isAirborne = false;
    private bool jumping = false;

    private bool movementEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (!movementEnabled)
        {
            return;
        }

        if (IsMoving())
        {
            characterTransform.rotation = Quaternion.LookRotation(movementVector);
        }
        characterController.Move(Time.deltaTime * movementSpeed * movementVector);

        isAirborne = !characterController.isGrounded;

        if (!isAirborne && verticalVelocity < 0f)
        {
            verticalVelocity = 0f;
            jumping = false;
        }
        verticalVelocity -= gravity * Time.deltaTime;
        characterController.Move(Time.deltaTime * verticalVelocity * Vector3.up);
    }

    public void Move(Vector2 inputedMovement)
    {
        Vector3 movementRight = GameManager.instance.cameraManager.GetCurrentProjectedRight().normalized * inputedMovement.x;
        Vector3 movementFront = GameManager.instance.cameraManager.GetCurrentProjectedFront().normalized * inputedMovement.y;

        movementVector = movementRight + movementFront;
    }

    public void Jump()
    {
        verticalVelocity = jumpSpeed;
        jumping = true;
    }

    public void TeleportPlayer(Vector3 position)
    {
        characterTransform.position = position;
    }

    public void OrientatePlayer(Quaternion rotation)
    {
        characterTransform.rotation = rotation;
    }

    public Vector3 GetPosition()
    {
        return characterTransform.position;
    }

    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }

    public bool IsAirbone()
    {
        return isAirborne && jumping;
    }

    public bool IsMoving()
    {
        return movementVector != Vector3.zero;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
        characterController.enabled = true;
    }

    public void DisableMovement()
    {
        movementEnabled = false;
        characterController.enabled = false;
    }
}
