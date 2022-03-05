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
    [SerializeField] private float groundedCheckDistance;
    [SerializeField] private LayerMask terrainMask;

    private Vector3 movementVector;

    private float verticalVelocity = 0f;
    private bool isGrounded = false;
    private bool isFalling = false;
    private bool isJumping = false;

    private bool isSignificantVerticalMovement = false;
    private const float FALLING_EPSILON = 0.025f;
    private float previousVerticalPosition;

    // Update is called once per frame
    void Update()
    {
        if (IsMoving())
        {
            characterTransform.rotation = Quaternion.LookRotation(movementVector);
        }
        characterController.Move(Time.deltaTime * movementSpeed * movementVector);


        isGrounded = IsGrounded();
        isFalling = verticalVelocity < 0f;
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else if (isFalling)
        {
            verticalVelocity = 0f;
            isJumping = false;
        }
        previousVerticalPosition = characterTransform.position.y;
        characterController.Move(Time.deltaTime * verticalVelocity * Vector3.up);
        isSignificantVerticalMovement = Mathf.Abs(previousVerticalPosition - characterTransform.position.y) > FALLING_EPSILON;
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
        isJumping = true;
    }

    public void Teleport(Vector3 position)
    {
        characterController.enabled = false;
        characterTransform.position = position;
        characterController.enabled = true;
    }

    public void Orientate(Quaternion rotation)
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
        return (!isGrounded && isSignificantVerticalMovement) || isJumping;
    }

    public bool IsMoving()
    {
        return movementVector != Vector3.zero;
    }

    private bool IsGrounded()
    {
        bool collision = Physics.Raycast(
            characterTransform.position,
            Vector3.down,
            groundedCheckDistance,
            terrainMask.value
        );

        return collision;
    }
}
