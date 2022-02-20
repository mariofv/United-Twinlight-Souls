using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class UIElement : MonoBehaviour
{
    [SerializeField] protected CanvasGroup uiElementCanvasGroup;
    
    protected List<TweeningAnimation> showTweens = new List<TweeningAnimation>();
    protected List<TweeningAnimation> hideTweens = new List<TweeningAnimation>();
    
    protected virtual void Start()
    {
        CreateShowTweens();
        CreateHideTweens();
    }

    public virtual void UpdateData() { }

    protected virtual void CreateShowTweens() { }
    public void Show(bool instant = false)
    {
        uiElementCanvasGroup.gameObject.SetActive(true);

        for (int i = 0; i < hideTweens.Count; ++i)
        {
            hideTweens[i].Rewind();
        }

        for (int i = 0; i < showTweens.Count; ++i)
        {
            showTweens[i].Rewind();
            showTweens[i].Play().SetInstantRun(instant);
        }

        ShowSpecialized(instant);
        UpdateData();
    }
    public virtual void ShowSpecialized(bool instant) { }

    protected virtual void CreateHideTweens() { }
    public void Hide(bool instant = false)
    {
        for (int i = 0; i < showTweens.Count; ++i)
        {
            showTweens[i].Rewind();
        }

        for (int i = 0; i < hideTweens.Count; ++i)
        {
            hideTweens[i].Rewind();
            hideTweens[i].Play().SetInstantRun(instant);
        }

        HideSpecialized(instant);
    }
    public virtual void HideSpecialized(bool instant) { }
}
