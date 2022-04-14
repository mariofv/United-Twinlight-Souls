using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnVFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems;
    [SerializeField] private float aliveTime;
    private float currentTime = 0f;
    private bool running = false;

    private void Update()
    {
        if (running)
        {
            currentTime += Time.deltaTime;
            if (currentTime > aliveTime)
            {
                running = false;
            }
        }
    }

    public void PlayEffect()
    {
        running = true;
        currentTime = 0f;
        for (int i = 0; i < particleSystems.Count; ++i)
        {
            particleSystems[i].Clear();
            particleSystems[i].Play();
        }
    }

    public bool IsAvailable()
    {
        return !running;
    }
}
