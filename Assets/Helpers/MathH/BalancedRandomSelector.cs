using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancedRandomSelector
{
    private int numberOfEvents;
    private float probabilityStep;

    private float originalProbability;
    private List<float> eventsProbabilities;
    private List<float> currentProbabilities;

    public BalancedRandomSelector(int numberOfEvents, float probabilityStep)
    {
        this.numberOfEvents = numberOfEvents;
        this.probabilityStep = probabilityStep;

        originalProbability = 1f / numberOfEvents;

        eventsProbabilities = new List<float>(new float[numberOfEvents]);
        ResetToOriginalProbability();

        currentProbabilities = new List<float>(new float[numberOfEvents]);
        BuildCurrentProbabilities();
    }

    public int Select()
    {
        float randomValue = Random.Range(0f, 1f);
        int selectedEvent = -1;
        for (int i = 0; i < numberOfEvents; ++i)
        {
            if (randomValue >= currentProbabilities[i])
            {
                selectedEvent = i;
            }
            else
            {
                break;
            }
        }

        BalanceProbabilities(selectedEvent);

        return selectedEvent;
    }

    private void BalanceProbabilities(int lastEvent)
    {
        if (eventsProbabilities[lastEvent] > originalProbability)
        {
            ResetToOriginalProbability();
        }

        float extractedProbability = Mathf.Min(eventsProbabilities[lastEvent], probabilityStep);
        for (int i = 0; i < numberOfEvents; ++i)
        {
            if (i != lastEvent)
            {
                eventsProbabilities[i] += extractedProbability / (numberOfEvents - 1);
            }
            else
            {
                eventsProbabilities[i] -= extractedProbability;
            }
        }

        BuildCurrentProbabilities();
    }

    private void ResetToOriginalProbability()
    {
        for (int i = 0; i < numberOfEvents; ++i)
        {
            eventsProbabilities[i] = originalProbability;
        }
    }

    private void BuildCurrentProbabilities()
    {
        currentProbabilities[0] = 0f;
        for (int i = 1; i < numberOfEvents; ++i)
        {
            currentProbabilities[i] = eventsProbabilities[i - 1] + currentProbabilities[i - 1];
        }
    }
}
