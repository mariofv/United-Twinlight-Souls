using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenThumbnail : MonoBehaviour
{
    private RectTransform rectTransform;
    private float originalPositionY;

    [SerializeField] private float bounceDelay;
    [SerializeField] private float bounceHeight;
    [SerializeField] private float bounceSpeed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPositionY = transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        float currentProgress = Mathf.Max(0f, Mathf.Sin(bounceDelay + Time.realtimeSinceStartup * bounceSpeed));
        newPosition.y = originalPositionY + bounceHeight * currentProgress;
        transform.position = newPosition;
    }
}
