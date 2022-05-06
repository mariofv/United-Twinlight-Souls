using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCInteraction npcInteraction;
    [SerializeField] private PlayerDetectionCollider playerDetection;
    [SerializeField] private float rotationSpeed;
    private Transform targetTransform;

    private void Awake()
    {
        playerDetection.onPlayerDetected.AddListener(LookAtTransform);
        playerDetection.onPlayerLost.AddListener(StopLookingAtPlayer);
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            return;
        }

        Vector3 lookDirection = targetTransform.position - transform.position;
        lookDirection.y = 0f;
        
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public NPCInteraction GetInteraction()
    {
        return npcInteraction;
    }

    public void LookAtTransform(Transform transform)
    {
        targetTransform = transform;
    }

    public void LookAtPlayer()
    {
        LookAtTransform(GameManager.instance.player.GetControlledCharacter().transform);
    }

    public void StopLookingAtPlayer()
    {
        targetTransform = null;
    }
}
