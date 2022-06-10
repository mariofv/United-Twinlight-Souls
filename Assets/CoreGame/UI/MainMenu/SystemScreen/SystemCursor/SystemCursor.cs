using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCursor : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Move(Vector2 position)
    {
        Vector2 newPosition = rectTransform.position;
        newPosition.y = position.y;
        rectTransform.position = newPosition;
    }
}
