using UnityEngine;
using System;

namespace FlipAnimator
{

    public class FlipTimescaleAnimation : IFlipAnimation
    {
        public delegate void FlipAnimationUpdateEvent();
        public event FlipAnimationUpdateEvent OnFrameUpdate;

        public uint AnimatedOn => _animatedOn;
        private uint _animatedOn;

        public uint AnimationLayer => _animationLayer;
        private uint _animationLayer;

        public float T { get; set; }

        public void ProgressFrame()
        {
            T += AnimatedOn / 24.0f;
            OnFrameUpdate?.Invoke();
        }

        public FlipTimescaleAnimation(uint animatedOn, uint layer)
        {
            _animatedOn = animatedOn;
            _animationLayer = layer;
        }
    }
}
