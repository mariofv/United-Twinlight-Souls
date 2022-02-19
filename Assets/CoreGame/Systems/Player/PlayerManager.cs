using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private CharacterController characterController;

    private Vector3 movementVector;
    private float verticalVelocity = 0f;
    private bool isAirborne = false;
    private bool jumping = false;
    private float previousVerticalPosition;
    private const float FALLING_EPSILON = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool significantVerticalMovement = Mathf.Abs(previousVerticalPosition - characterTransform.position.y) > FALLING_EPSILON;
        isAirborne = !characterController.isGrounded;

        if (!isAirborne  && verticalVelocity < 0f)
        {
            verticalVelocity = 0f;
            jumping = false;
        }

        characterAnimator.SetBool("isAirborne", isAirborne && jumping);

        if (movementVector != Vector3.zero)
        {
            characterTransform.rotation = Quaternion.LookRotation(movementVector);
        }

        characterController.Move(Time.deltaTime * movementSpeed * movementVector);
        
        verticalVelocity -= gravity * Time.deltaTime;
        characterAnimator.SetFloat("verticalVelocity", verticalVelocity);

        previousVerticalPosition = characterTransform.position.y;
        characterController.Move(Time.deltaTime * verticalVelocity * Vector3.up);
    }

    public void Move(Vector2 inputedMovement)
    {
        Vector3 movementRight = GameManager.instance.cameraManager.GetCurrentProjectedRight().normalized * inputedMovement.x;
        Vector3 movementFront = GameManager.instance.cameraManager.GetCurrentProjectedFront().normalized * inputedMovement.y;

        movementVector = movementRight + movementFront;

        characterAnimator.SetBool("moving", movementVector != Vector3.zero);
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

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
