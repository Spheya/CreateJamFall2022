using UnityEngine;
using System;

namespace FlipAnimator
{
    [Serializable]
    public class FlipAnimation : IFlipAnimation
    {
        public delegate void FlipAnimationUpdateEvent();
        public event FlipAnimationUpdateEvent OnFrameUpdate;

        [SerializeField]
        private FlipAnimationSequence _sequence;
        public FlipAnimationSequence Sequence => _sequence;

        public uint AnimatedOn => _sequence.AnimatedOn;

        public uint AnimationLayer => _animationLayer;
        private uint _animationLayer;

        public int FrameIndex { get; set; }
        public FlipAnimationSequence.Frame Frame => _sequence[FrameIndex];

        public void ProgressFrame()
        {
            FrameIndex = (FrameIndex + 1) % _sequence.Count;
            OnFrameUpdate?.Invoke();
        }

        public FlipAnimation(FlipAnimationSequence sequence, uint layer)
        {
            _sequence = sequence;
            _animationLayer = layer;
        }
    }
}
