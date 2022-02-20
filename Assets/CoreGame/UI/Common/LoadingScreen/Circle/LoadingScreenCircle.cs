using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenCircle : MonoBehaviour
{
    private class LoadingScreenCircleDot
    {
        public RectTransform dotTransform;
        public float offset = 0f;
    }

    [SerializeField] private GameObject dotPrefab;
    private List<LoadingScreenCircleDot> circleDots;

    [SerializeField] private int numberOfPoints;
    [SerializeField] private float radius;

    [SerializeField] private float expansionRadius;
    [SerializeField] private float expansionSpeed;



    // Start is called before the first frame update
    void Start()
    {
        circleDots = new List<LoadingScreenCircleDot>();

        RectTransform rectTransform = GetComponent<RectTransform>();
        float angleStep = 360 / numberOfPoints;

        for(int i = 0; i < numberOfPoints; ++i)
        {
            LoadingScreenCircleDot currentDot = new LoadingScreenCircleDot();

            currentDot.dotTransform = Instantiate(dotPrefab, transform).GetComponent<RectTransform>();

            Quaternion dotRotation = Quaternion.Euler(0f, 0f, angleStep * i);
            currentDot.dotTransform.position = rectTransform.position + (dotRotation * Vector3.up * radius);
            currentDot.offset = ((float)i / numberOfPoints) * 2 * Mathf.PI;

            circleDots.Add(currentDot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numberOfPoints; ++i)
        {
            float currentProgress = Mathf.Max(0f, Mathf.Sin(circleDots[i].offset + Time.realtimeSinceStartup * expansionSpeed));
            circleDots[i].dotTransform.sizeDelta = Mathf.Lerp(20, expansionRadius, currentProgress) * Vector2.one;
        }
    }
}
