using UnityEngine;
using UnityEngine.UI;
using System;

namespace Tweening
{
    public abstract class TweeningAnimation
    {
        private Guid animationGUID;

        private float targetTime = 0f;
        private float currentTime = 0f;
        private bool isRunning = false;

        private bool isAlive = false;
        private bool isKilledOnEnd = true;
        private bool isUnscaled = false;
        private bool isInstantRun = false;

        public TweeningAnimation()
        {
            GenerateGUID();
        }

        public void GenerateGUID()
        {
            animationGUID = Guid.NewGuid();
        }

        public Guid GetGUID()
        {
            return animationGUID;
        }

        public void Run(float targetTime)
        {
            this.targetTime = targetTime;

            currentTime = 0f;
            isRunning = true;
            isAlive = true;
        }

        public TweeningAnimation Play()
        {
            isRunning = true;

            return this;
        }

        public TweeningAnimation Pause()
        {
            isRunning = false;

            return this;
        }

        public TweeningAnimation Rewind()
        {
            currentTime = 0f;
            isRunning = false;
            UpdateSpecific(0f);

            return this;
        }

        public void Update(float deltaTime)
        {
            if (!isRunning)
            {
                return;
            }

            float progress;
            if (isInstantRun)
            {
                progress = 1f;
            }
            else
            {
                currentTime += deltaTime;
                progress = Mathf.Min(1f, currentTime / targetTime);
            }

            UpdateSpecific(progress);

            if (progress == 1f)
            {
                isRunning = false;
                isAlive = !isKilledOnEnd;
            }
        }

        protected abstract void UpdateSpecific(float progress);

        public TweeningAnimation Unscaled()
        {
            isUnscaled = true;
            return this;
        }

        public bool IsUnscaled()
        {
            return isUnscaled;
        }

        public TweeningAnimation SetInstantRun(bool instantRun)
        {
            isInstantRun = instantRun;
            return this;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void Kill()
        {
            isRunning = false;
            isAlive = false;
        }

        public TweeningAnimation DontKillOnEnd()
        {
            isKilledOnEnd = false;
            return this;
        }

        public bool IsAlive()
        {
            return isAlive;
        }
    }
    //---------------------------------------------------------------------------------
    public class DisableGameObjectTweeningAnimation : TweeningAnimation
    {
        public GameObject gameObject;

        protected override void UpdateSpecific(float progress)
        {
            if (progress == 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    //---------------------------------------------------------------------------------
    public class FadeTweeningAnimation : TweeningAnimation
    {
        public Image image;
        public CanvasGroup canvasGroup;
        public float fromValue;
        public float toValue;

        protected override void UpdateSpecific(float progress)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(fromValue, toValue, progress);
            }

            if (image!= null)
            {
                Color newColor = image.color;
                newColor.a = Mathf.Lerp(fromValue, toValue, progress);
                image.color = newColor;
            }
        }
    }
    //---------------------------------------------------------------------------------
    public class SlideTweeningAnimation : TweeningAnimation
    {
        public Vector2 initPosition;
        public Vector2 targetPosition;
        public RectTransform rectTransform;

        protected override void UpdateSpecific(float progress)
        {
            rectTransform.position = Vector2.Lerp(initPosition, targetPosition, progress);
        }
    }
    //---------------------------------------------------------------------------------
}
