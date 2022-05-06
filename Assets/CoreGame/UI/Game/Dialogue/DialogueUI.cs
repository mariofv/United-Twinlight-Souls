using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;
using TMPro;

public class DialogueUI : UIElement
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextTyper dialogueTextTyper;

    public void DisplayDialogueText(string text)
    {
        dialogueText.text = text;
        dialogueTextTyper.StartTyping();
    }

    public void SkipDialogueTyping()
    {
        dialogueTextTyper.EndTyping();
    }

    public bool IsTyping()
    {
        return dialogueTextTyper.IsTyping();
    }

    protected override void CreateShowTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 0f, 1f).DontKillOnEnd();
        showTweens.Add(fadeAnimation);
    }

    protected override void CreateHideTweens()
    {
        TweeningAnimation fadeAnimation = uiElementCanvasGroup.TweenFade(UISettings.GameUISettings.DISPLAY_TIME, 1f, 0f).DontKillOnEnd();
        hideTweens.Add(fadeAnimation);

        TweeningAnimation disableGameObjectAnimation = uiElementCanvasGroup.gameObject.TweenDisable(UISettings.GameUISettings.DISPLAY_TIME).DontKillOnEnd();
        hideTweens.Add(disableGameObjectAnimation);
    }
}
