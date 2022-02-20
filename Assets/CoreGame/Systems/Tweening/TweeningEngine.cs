using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tweening
{
    public class TweeningEngine : MonoBehaviour
    {
        public static TweeningEngine instance;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                tweeningAnimations = new Dictionary<System.Guid, TweeningAnimation>();

                slideTweeningAnimations = new TweeningAnimationPool<SlideTweeningAnimation>();
                slideTweeningAnimations.Init(tweeningAnimations, 10);

                disableGameObjectTweeningAnimations = new TweeningAnimationPool<DisableGameObjectTweeningAnimation>();
                disableGameObjectTweeningAnimations.Init(tweeningAnimations, 10);

                fadeTweeningAnimations = new TweeningAnimationPool<FadeTweeningAnimation>();
                fadeTweeningAnimations.Init(tweeningAnimations, 10);
            }
        }

        private Dictionary<System.Guid, TweeningAnimation> tweeningAnimations;

        private TweeningAnimationPool<SlideTweeningAnimation> slideTweeningAnimations;
        private TweeningAnimationPool<DisableGameObjectTweeningAnimation> disableGameObjectTweeningAnimations;
        private TweeningAnimationPool<FadeTweeningAnimation> fadeTweeningAnimations;

        // Update is called once per frame
        void Update()
        {
            foreach (TweeningAnimation currentAnimation in tweeningAnimations.Values)
            {
                if (!currentAnimation.IsRunning())
                {
                    continue;
                }

                if (currentAnimation.IsUnscaled())
                {
                    currentAnimation.Update(Time.unscaledDeltaTime);
                }
                else
                {
                    currentAnimation.Update(Time.deltaTime);
                }
            }
        }

        public static void Kill(System.Guid tweeningAnimationGUID)
        {
            if (!instance.tweeningAnimations.ContainsKey(tweeningAnimationGUID))
            {
                throw new UnityException("Trying to kill a non existing tweening animation!");
            }
            instance.tweeningAnimations[tweeningAnimationGUID].Kill();
        }

        public static SlideTweeningAnimation Slide(float targetTime, RectTransform transform, Vector2 initPosition, Vector2 targetPosition)
        {
            SlideTweeningAnimation slideTweeningAnimation = instance.slideTweeningAnimations.GetElement();
            slideTweeningAnimation.initPosition = initPosition;
            slideTweeningAnimation.targetPosition = targetPosition;
            slideTweeningAnimation.rectTransform = transform;

            slideTweeningAnimation.Run(targetTime);

            return slideTweeningAnimation;
        }

        public static DisableGameObjectTweeningAnimation DisableGameObject(float targetTime, GameObject gameObject)
        {
            DisableGameObjectTweeningAnimation disableGameObjectTweeningAnimation = instance.disableGameObjectTweeningAnimations.GetElement();
            disableGameObjectTweeningAnimation.gameObject = gameObject;

            disableGameObjectTweeningAnimation.Run(targetTime);

            return disableGameObjectTweeningAnimation;
        }

        public static FadeTweeningAnimation Fade(float targetTime, CanvasGroup canvasGroup, float fromValue, float toValue)
        {
            FadeTweeningAnimation fadeTweeningAnimation = instance.fadeTweeningAnimations.GetElement();
            fadeTweeningAnimation.canvasGroup = canvasGroup;
            fadeTweeningAnimation.fromValue = fromValue;
            fadeTweeningAnimation.toValue = toValue;

            fadeTweeningAnimation.Run(targetTime);

            return fadeTweeningAnimation;
        }
    }
}