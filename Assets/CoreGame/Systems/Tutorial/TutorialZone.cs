using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [SerializeField] private Tutorial zoneTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            GameManager.instance.tutorialManager.StartTutorial(zoneTutorial);
        }
    }
}
