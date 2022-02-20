using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweening
{
    public class TweeningAnimationPool<T> where T : TweeningAnimation, new()
    {
        private Dictionary<System.Guid, TweeningAnimation> globalTweeningAnimations;
        private List<T> tweeningAnimations;
        private int allocationBatchSize;

        public void Init(Dictionary<System.Guid, TweeningAnimation> globalTweeningAnimations, int poolSize, int allocationBatchSize = 5)
        {
            this.globalTweeningAnimations = globalTweeningAnimations;
            this.allocationBatchSize = allocationBatchSize;

            tweeningAnimations = new List<T>();
            AllocateElements(poolSize);
        }

        public T GetElement()
        {
            int poolSize = tweeningAnimations.Count;
            for (int i = 0; i < poolSize; ++i)
            {
                if (!tweeningAnimations[i].IsAlive())
                {
                    return tweeningAnimations[i];
                }
            }

            AllocateElements(allocationBatchSize);
            return tweeningAnimations[poolSize];
        }

        private void AllocateElements(int numElements)
        {
            for (int i = 0; i < numElements; ++i)
            {
                T newTweeningAnimation = new T();
                tweeningAnimations.Add(newTweeningAnimation);

                while (globalTweeningAnimations.ContainsKey(newTweeningAnimation.GetGUID()))
                {
                    newTweeningAnimation.GenerateGUID();
                }
                globalTweeningAnimations.Add(newTweeningAnimation.GetGUID(), newTweeningAnimation);
            }
        }
    }
}
