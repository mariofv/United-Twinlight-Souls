using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathH
{
    public class Line
    {
        public Vector3 pointA;
        public Vector3 pointB;
        private Vector3 ABVector;

        public Line()
        {
        }

        public Line (Vector3 a, Vector3 b)
        {
            pointA = a;
            pointB = b;

            ABVector = pointB - pointA;
        }

        public Line Project(Vector3 position)
        {
            Vector3 AP = position - pointA;
            Vector3 projection = pointA + Vector3.Dot(AP, ABVector) / Vector3.Dot(ABVector, ABVector) * ABVector;

            return new Line(position, projection);
        }

        public bool Contains(Vector3 position)
        {
            float crossProduct = (position.y - pointA.y) * (pointB.x - pointA.x) - (position.x - pointA.x) * (pointB.y - pointA.y);

            if (Mathf.Abs(crossProduct) > float.Epsilon)
            {
                return false;
            }

            float dotProduct = (position.x - pointA.x) * (pointB.x - pointA.x) + (position.y - pointA.y) * (pointB.y - pointA.y);
            if (dotProduct < 0f)
            {
                return false;
            }

            if (dotProduct > ABVector.sqrMagnitude)
            {
                return false;
            }

            return true;
        }

    }
}
