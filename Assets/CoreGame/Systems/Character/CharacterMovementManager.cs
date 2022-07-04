using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : CharacterSubManager
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;

    [SerializeField] private LayerMask terrainMask;

    [Header("Ground movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Air movement")]
    [SerializeField] private float gravity;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float groundedCheckDistance;

    [Header("Dash")]
    [SerializeField] private float dashSpeedMultiplier;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float currentDashTime = 0f;
    private float currentDashCooldown = 0f;

    private Vector3 movementVector;
    private bool isMoving = false;

    private float verticalVelocity = 0f;
    private bool isGrounded = false;
    private bool isFalling = false;
    private bool isJumping = false;
    private bool isDashing = false;

    private bool isSignificantVerticalMovement = false;
    private const float FALLING_EPSILON = 0.025f;
    private float previousVerticalPosition;

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            currentDashTime += Time.deltaTime;
            if (currentDashTime >= dashDuration)
            {
                isDashing = false;
                characterManager.EndDash();
            }
        }

        if (currentDashCooldown > 0f)
        {
            currentDashCooldown = Mathf.Max(0f, currentDashCooldown - Time.deltaTime);
            GameManager.instance.uiManager.gameUIManager.hudUI.SetDashProgress(1f - currentDashCooldown / dashCooldown);
        }

        if (IsMoving())
        {
            Quaternion lookRotation = Quaternion.LookRotation(movementVector);
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        Vector3 horizontalMovement = ComputeHorizontalMovement();
        Vector3 verticalMovement = ComputeVerticalMovement();

        previousVerticalPosition = characterTransform.position.y;
        characterController.Move(Time.deltaTime * (horizontalMovement + verticalMovement));
        isSignificantVerticalMovement = Mathf.Abs(previousVerticalPosition - characterTransform.position.y) > FALLING_EPSILON;
    }

    public void SetInputedMovement(Vector2 inputedMovement, bool adjustToCamera = true)
    {
        Vector3 movementRight;
        Vector3 movementFront;
        if (adjustToCamera)
        {
            movementRight = GameManager.instance.cameraManager.GetCurrentProjectedRight().normalized * inputedMovement.x;
            movementFront = GameManager.instance.cameraManager.GetCurrentProjectedFront().normalized * inputedMovement.y;
        }
        else
        {
            movementRight = Vector3.right * inputedMovement.x;
            movementFront = Vector3.forward * inputedMovement.y;
        }

        movementVector = movementRight + movementFront;
        isMoving = movementVector != Vector3.zero;
    }

    public void Jump()
    {
        if (isJumping)
        {
            return;
        }

        verticalVelocity = jumpSpeed;
        isJumping = true;
    }

    public bool CanDash()
    {
        return currentDashCooldown == 0f;
    }

    public void EnableDashMultiplier()
    {
        isDashing = true;
        currentDashCooldown = dashCooldown;
        currentDashTime = 0f;
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

    public void Orientate(Transform targetTransform)
    {
        Vector3 lookDirection = (targetTransform.position - transform.position).normalized;
        lookDirection.y = 0;
        Orientate(Quaternion.LookRotation(lookDirection));
    }

    private Vector3 ComputeHorizontalMovement()
    {
        float movementMultiplier = 1f;
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (GameManager.instance.debugManager.flashMode)
            {
                movementMultiplier = 5f;
            }
        }

        if (isDashing)
        {
            movementMultiplier *= dashSpeedMultiplier;
        }

        return movementMultiplier * movementSpeed * movementVector;
    }

    private Vector3 ComputeVerticalMovement()
    {
        isGrounded = IsGrounded();
        isFalling = verticalVelocity < 0f;
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            if (isFalling)
            {
                verticalVelocity = 0f;
                isJumping = false;
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }

        if (isDashing)
        {
            verticalVelocity = 0f;
        }

        return verticalVelocity * Vector3.up;
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
        return isMoving;
    }

    public bool IsJumping()
    {
        return isJumping;
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
