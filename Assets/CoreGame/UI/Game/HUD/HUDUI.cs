using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDUI : UIElement
{
    [SerializeField] private GameObject baraldPortrait;
    [SerializeField] private GameObject ilonaPortrait;

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
