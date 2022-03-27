using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float showTime;
    [SerializeField] private float fadingTime;
    [SerializeField] private float finalScale;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float gravity;

    private RectTransform rectTransform;
    private TextMeshProUGUI text;
    private bool alive = false;
    private float currentTime = 0f;
    private Vector3 originalPosition;
    private Vector2 direction;

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            currentTime += Time.deltaTime;
            Vector2 canvasPosition = GameManager.instance.cameraManager.mainCamera.WorldToScreenPoint(originalPosition);

            float positionX = canvasPosition.x + direction.x * horizontalSpeed * currentTime;
            float positionY = canvasPosition.y + direction.y + verticalSpeed * currentTime + 0.5f * (-gravity) * currentTime * currentTime;

            rectTransform.position = new Vector2(positionX, positionY);

            float elapsedFadingTime = Mathf.Max(0f, currentTime - showTime);
            float progress = elapsedFadingTime / fadingTime;

            text.color = new Color(1f, 1f, 1f, 1f - progress + 0.2f);
            rectTransform.localScale = new Vector3(1f - progress + 0.2f, 1f - progress + 0.2f, 1f);

            if (currentTime >= fadingTime + showTime)
            {
                alive = false;
                Destroy(transform.gameObject);
            }
        }
    }

    public void Spawn(int number, Vector3 damageInflicterPosition, Vector3 damageReceiverPosition)
    {
        alive = true;
        currentTime = 0f;
        text.text = number.ToString();
        originalPosition = damageReceiverPosition;

        Vector2 inflicterPositionScreenSpace = GameManager.instance.cameraManager.mainCamera.WorldToScreenPoint(damageInflicterPosition);
        Vector2 receiverPositionScreenSpace = GameManager.instance.cameraManager.mainCamera.WorldToScreenPoint(damageReceiverPosition);
        direction = (receiverPositionScreenSpace - inflicterPositionScreenSpace).normalized;

        rectTransform.position = receiverPositionScreenSpace;
    }
}
