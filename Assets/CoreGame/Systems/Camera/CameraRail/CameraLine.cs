using System.Collections.Generic;
using UnityEngine;

public class CameraLine : MonoBehaviour
{
    [SerializeField] private Transform pointsParent;

    private List<Vector3> pointsPositions;
    private List<float> pointsAccumulatedDistance;
    private float lineTotalDistance;

    private void Awake()
    {
        pointsPositions = new List<Vector3>();
        pointsAccumulatedDistance = new List<float>();
        for (int i = 0; i < pointsParent.childCount; ++i)
        {
            pointsPositions.Add(pointsParent.GetChild(i).transform.position);

            if (i == 0)
            {
                pointsAccumulatedDistance.Add(0f);
            }
            else
            {
                pointsAccumulatedDistance.Add(pointsAccumulatedDistance[i - 1] + Vector3.Distance(pointsPositions[i], pointsPositions[i - 1]));
            }
        }

        lineTotalDistance = pointsAccumulatedDistance[pointsAccumulatedDistance.Count - 1];
    }


    public float GetProgress(Vector3 position)
    {
        int closestPointIndex = GetClosestPointIndex(position);
        MathH.Line linePerpendicularToCurrentLine;

        MathH.Line currentLine;
        int lowerLimitIndex = -1;

        if (closestPointIndex == 0)
        {
            lowerLimitIndex = 0;
            currentLine = new MathH.Line(pointsPositions[0], pointsPositions[1]);
            linePerpendicularToCurrentLine = currentLine.Project(position);

        }
        else if (closestPointIndex == pointsPositions.Count - 1)
        {
            int secondToLastPositionIndex = pointsPositions.Count - 2;
            lowerLimitIndex = secondToLastPositionIndex;
            currentLine = new MathH.Line(pointsPositions[secondToLastPositionIndex], pointsPositions[pointsPositions.Count - 1]);
            linePerpendicularToCurrentLine = currentLine.Project(position);

        }
        else
        {
            MathH.Line leftSegment = new MathH.Line(pointsPositions[closestPointIndex - 1], pointsPositions[closestPointIndex]);
            MathH.Line perpendicularToLeftSegment = leftSegment.Project(position);
            bool isInLeftSegment = leftSegment.Contains(perpendicularToLeftSegment.pointB);

            MathH.Line rightSegment = new MathH.Line(pointsPositions[closestPointIndex], pointsPositions[closestPointIndex + 1]);
            MathH.Line perpendicularToRightSegment = rightSegment.Project(position);
            bool isInRightSegment = rightSegment.Contains(perpendicularToRightSegment.pointB);

            if (isInRightSegment && !isInRightSegment)
            {
                lowerLimitIndex = closestPointIndex;
                currentLine = rightSegment;
                linePerpendicularToCurrentLine = perpendicularToRightSegment;
            }
            else if (!isInRightSegment && isInLeftSegment)
            {
                lowerLimitIndex = closestPointIndex - 1;
                currentLine = leftSegment;
                linePerpendicularToCurrentLine = perpendicularToLeftSegment;
            }
            else
            {
                throw new UnityException("The player is in both line segments!");
            }
        }

        Vector3 projectedPosition = linePerpendicularToCurrentLine.pointB;

        float distanceToCurrentLineClosestPoint = Vector3.Distance(projectedPosition, currentLine.pointA);
        float distanceToStartOfTheLine = pointsAccumulatedDistance[lowerLimitIndex] + distanceToCurrentLineClosestPoint;

        return distanceToStartOfTheLine / lineTotalDistance;
    }


    public Vector3 GetPoint(float progress)
    {
        if (progress <= 0f)
        {
            return pointsPositions[0];
        }
        if (progress >= 1f)
        {
            return pointsPositions[pointsPositions.Count - 1];
        }

        float currentDistance = lineTotalDistance * progress;
        bool found = false;
        int i = 0;
        while (!found && i < pointsAccumulatedDistance.Count)
        {
            if (pointsAccumulatedDistance[i] < currentDistance)
            {
                ++i;
            }
            else
            {
                found = true;
            }
        }

        float currentPercentageInLine = (currentDistance - pointsAccumulatedDistance[i - 1]) / (pointsAccumulatedDistance[i] - pointsAccumulatedDistance[i - 1]);

        return Vector3.Lerp(pointsPositions[i - 1], pointsPositions[i], currentPercentageInLine);
    }


    private int GetClosestPointIndex(Vector3 position)
    {
        int closestPointIndex = -1;
        float minDistance = 10000f;

        for (int i = 0; i < pointsPositions.Count; ++i)
        {
            float currentDistance = Vector3.Distance(pointsPositions[i], position);

            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestPointIndex = i;
            }
        }
        return closestPointIndex;
    }
}
