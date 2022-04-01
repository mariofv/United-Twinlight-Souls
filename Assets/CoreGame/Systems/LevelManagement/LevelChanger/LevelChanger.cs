using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private int levelToTransition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagManager.PLAYER)
        {
            GameManager.instance.levelManager.LoadLevel(levelToTransition, waitLoadingScreenTime: true);
        }
    }
}
