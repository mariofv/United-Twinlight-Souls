using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private PlayerDetectionCollider playerDetection;
    [SerializeField] private float rotationSpeed;
    private Transform playerTransform;

    private void Awake()
    {
        playerDetection.onPlayerDetected.AddListener(StartLookingAtPlayer);
        playerDetection.onPlayerLost.AddListener(StopLookingAtPlayer);
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0f;
        
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void StartLookingAtPlayer(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public void StopLookingAtPlayer()
    {
        playerTransform = null;
    }
}
