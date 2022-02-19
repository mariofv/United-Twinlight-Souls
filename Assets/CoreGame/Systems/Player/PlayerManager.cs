using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Transform characterTransform;

    public float movementSpeed;
    private Vector3 movementVector;

    private void Awake()
    {
        characterTransform = transform;
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
        transform.position += Time.fixedDeltaTime * movementSpeed * new Vector3(movementVector.x, 0f, movementVector.z);
    }

    public void Move(Vector2 inputedMovement)
    {
        Vector3 movementRight = GameManager.instance.cameraManager.GetCurrentProjectedRight().normalized * inputedMovement.x;
        Vector3 movementFront = GameManager.instance.cameraManager.GetCurrentProjectedFront().normalized * inputedMovement.y;

        movementVector = movementRight + movementFront;
    }


    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
