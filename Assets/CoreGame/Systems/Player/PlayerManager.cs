using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody characterRigidBody;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    private Vector3 movementVector;

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

        characterRigidBody.position += Time.fixedDeltaTime * movementSpeed * movementVector;
    }

    public void Move(Vector2 inputedMovement)
    {
        Vector3 movementRight = GameManager.instance.cameraManager.GetCurrentProjectedRight().normalized * inputedMovement.x;
        Vector3 movementFront = GameManager.instance.cameraManager.GetCurrentProjectedFront().normalized * inputedMovement.y;

        movementVector = movementRight + movementFront;
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
