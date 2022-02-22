using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.


public class UISelectableSound : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        GameManager.instance.audioManager.PlayUISound(AudioManager.UISound.SELECT);
    }
}
