using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
