using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweening;

public class HUDUI : UIElement
{
    [Header("Portraits")]
    [SerializeField] private GameObject baraldPortrait;
    [SerializeField] private GameObject ilonaPortrait;

    [Header("Health Bar")]
    [SerializeField] private Image healthBarFill;

    [Header("Light Bar")]
    [SerializeField] private Image lightBarFill;

    public override void ShowSpecialized(bool instant)
    {
        if (GameManager.instance.player.GetControlledCharacterId() == Character.CharacterId.BARALD)
        {
            SelectBaraldPortrait();
        }
        else if (GameManager.instance.player.GetControlledCharacterId() == Character.CharacterId.ILONA)
        {
            SelectIlonaPortrait();
        }
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

    public void SetHealth(float healthPercentage)
    {
        healthBarFill.fillAmount = healthPercentage;
    }

    public void SelectBaraldPortrait()
    {
        baraldPortrait.SetActive(true);
        ilonaPortrait.SetActive(false);
    }

    public void SelectIlonaPortrait()
    {
        baraldPortrait.SetActive(false);
        ilonaPortrait.SetActive(true);
    }
}
