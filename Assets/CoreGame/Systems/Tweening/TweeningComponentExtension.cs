using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public static class TweeningComponentExtension
{
    public static SlideTweeningAnimation TweenSlide(this RectTransform transform, float targetTime, Vector2 initPosition, Vector2 targetPosition)
    {
        return TweeningEngine.Slide(targetTime, transform, initPosition, targetPosition);
    }

    public static DisableGameObjectTweeningAnimation TweenDisable(this GameObject gameObject, float targetTime)
    {
        return TweeningEngine.DisableGameObject(targetTime, gameObject);
    }

    public static TweeningAnimation TweenFade(this CanvasGroup canvasGroup, float time, float fromValue, float toValue)
    {
        return TweeningEngine.Fade(time, canvasGroup, fromValue, toValue);
    }
}
