using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttacksSelector : MonoBehaviour
{
    [SerializeField] private int numberOfAttacks;
    [SerializeField] private float probabilityStep;

    private float originalProbability;
    private List<float> attackProbabilities;
    private List<float> currentProbabilities;

    private void Awake()
    {
        originalProbability = 1f / numberOfAttacks;

        attackProbabilities = new List<float>(new float[numberOfAttacks]);
        ResetToOriginalProbability();

        currentProbabilities = new List<float>(new float[numberOfAttacks]);
        BuildCurrentProbabilities();
    }

    public int SelectAttack()
    {
        float randomValue = Random.Range(0f, 1f);
        int selectedAttack = -1;
        for (int i = 0; i < numberOfAttacks; ++i)
        {
            if (randomValue >= currentProbabilities[i])
            {
                selectedAttack = i;
            }
            else
            {
                break;
            }
        }

        BalanceProbabilities(selectedAttack);

        return selectedAttack;
    }

    private void BalanceProbabilities(int lastAttack)
    {
        if (attackProbabilities[lastAttack] > originalProbability)
        {
            ResetToOriginalProbability();
        }

        float extractedProbability = Mathf.Min(attackProbabilities[lastAttack], probabilityStep);
        for (int i = 0; i < numberOfAttacks; ++i)
        {
            if (i != lastAttack)
            {
                attackProbabilities[i] += extractedProbability / (numberOfAttacks - 1);
            }
            else
            {
                attackProbabilities[i] -= extractedProbability;
            }
        }

        BuildCurrentProbabilities();
    }

    private void ResetToOriginalProbability()
    {
        for (int i = 0; i < numberOfAttacks; ++i)
        {
            attackProbabilities[i] = originalProbability;
        }
    }

    private void BuildCurrentProbabilities()
    {
        currentProbabilities[0] = 0f;
        for (int i = 1; i < numberOfAttacks; ++i)
        {
            currentProbabilities[i] = attackProbabilities[i - 1] + currentProbabilities[i - 1];
        }
    }
}
