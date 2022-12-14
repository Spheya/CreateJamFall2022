using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FlipAnimator
{
    public class FlipAnimator : MonoBehaviour
    {
        private static FlipAnimator _instance;
        private static FlipAnimator Instance
        {
            get
            {
                if(_instance == null)
                {
                    GameObject obj = new GameObject("FlipAnimator [autogenerated]");
                    _instance = obj.AddComponent<FlipAnimator>();
                }

                return _instance;
            }
        }

        private List<IFlipAnimation> _animations;
        private ulong _frame;

        private void OnEnable()
        {
            _animations = new List<IFlipAnimation>();
            StartCoroutine(AnimationCoroutine());
        }

        private void OnDisable()
        {
            _animations.Clear();
            StopAllCoroutines();
        }

        public static void StartAnimation(IFlipAnimation animation)
        {
            Instance._animations.Add(animation);
        }

        public static void StopAnimation(IFlipAnimation animation)
        {
            if(_instance != null)
                Instance._animations.Remove(animation);
        }

        private IEnumerator AnimationCoroutine()
        {
            _frame = 0;
            while(true)
            {
                foreach(var animation in _animations)
                    if(_frame % animation.AnimatedOn == animation.AnimationLayer)
                        animation.ProgressFrame();

                yield return new WaitForSecondsRealtime(1.0f / 24.0f);
                ++_frame;
            }
        }
    }
}