using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemCursor : MonoBehaviour
{
    [SerializeField] private Image bloomImage;
    [SerializeField] private float bloomSpeed;
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

    private void Update()
    {
        bloomImage.SetAlpha((Mathf.Sin(Time.unscaledTime * bloomSpeed) + 1) * 0.25f);
    }
}
