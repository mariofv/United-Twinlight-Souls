using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class SelectCharacterScreenUIManager : MainMenuScreenUIManager
{
    [SerializeField] private CharacterSelectionCursor characterSelectionCursor;
    private bool baraldSelected = true;

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).DontKillOnEnd();
        showTweens.Add(fadeAnimation);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd();
        hideTweens.Add(fadeAnimation);
    }

    private void MoveCursorToBarald()
    {
        characterSelectionCursor.SelectBarald();
    }

    private void MoveCursorToIlona()
    {
        characterSelectionCursor.SelectIlona();
    }

    private void MoveCursor()
    {
        baraldSelected = !baraldSelected;
        if (baraldSelected)
        {
            MoveCursorToBarald();
        }
        else
        {
            MoveCursorToIlona();
        }
    }

    public override void OnLeftPressed()
    {
        if (baraldSelected)
        {
            MoveCursor();
        }
    }

    public override void OnRightPressed()
    {
        if (!baraldSelected)
        {
            MoveCursor();
        }
    }
}
