using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Transform characterTransform;

    public float movementSpeed;
    private Vector2 movementVector;

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
        transform.position += Time.fixedDeltaTime * movementSpeed * new Vector3(movementVector.x, 0f, movementVector.y);
    }

    public void Move(Vector2 inputedMovement)
    {
        movementVector = inputedMovement.normalized;
    }
}
