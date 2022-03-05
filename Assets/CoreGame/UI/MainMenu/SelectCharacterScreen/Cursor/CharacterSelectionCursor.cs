using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionCursor : MonoBehaviour
{
    [Header("Bloom")]
    [SerializeField] private Image cursorBloom;
    [SerializeField] private float blurSpeed;

    [Header("Character Info")]
    [SerializeField] private Image baraldIcon;
    [SerializeField] private Image ilonaIcon;
    [SerializeField] private TextMeshProUGUI characterNameText;

    [SerializeField] private RectTransform baraldCursorPosition;
    [SerializeField] private RectTransform ilonaCursorPosition;
    
    private RectTransform cursorRectTransform;

    private void Awake()
    {
        cursorRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Color newColor = cursorBloom.color;
        newColor.a = (Mathf.Sin(Time.time * blurSpeed) + 1) * 0.25f;
        cursorBloom.color = newColor;
    }

    public void SelectBarald()
    {
        characterNameText.text = "BARALD";
        ilonaIcon.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f);
        baraldIcon.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f);

        cursorRectTransform.TweenSlide(UISettings.GameUISettings.DISPLAY_TIME, ilonaCursorPosition.position, baraldCursorPosition.position);
    }
    public void SelectIlona()
    {
        characterNameText.text = "ILONA";
        ilonaIcon.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f);
        baraldIcon.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f);

        cursorRectTransform.TweenSlide(UISettings.GameUISettings.DISPLAY_TIME, baraldCursorPosition.position, ilonaCursorPosition.position);
    }
}
