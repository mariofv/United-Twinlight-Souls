using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody characterRigidBody;
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    private Vector3 movementVector;

    private const float VERTICAL_VELOCITY_EPSILON = 0.00001f;

    private void Awake()
    {
        characterRigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (movementVector != Vector3.zero)
        {
            characterRigidBody.rotation = Quaternion.LookRotation(movementVector);
        }

        float verticalVelocity = characterRigidBody.velocity.y;
        bool isAirborne = Mathf.Abs(verticalVelocity) > VERTICAL_VELOCITY_EPSILON;
        if (isAirborne)
        {
            characterAnimator.SetFloat("verticalVelocity", verticalVelocity);
        }
        characterAnimator.SetBool("isAirborne", isAirborne);

        characterRigidBody.position += Time.fixedDeltaTime * movementSpeed * movementVector;
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
        characterRigidBody.AddForce(Vector3.up * jumpForce);
    }


    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
