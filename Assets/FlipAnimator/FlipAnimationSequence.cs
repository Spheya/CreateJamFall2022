using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace FlipAnimator
{
    [CreateAssetMenu(menuName = "Flip Animation Sequence")]
    public class FlipAnimationSequence : ScriptableObject, IReadOnlyList<FlipAnimationSequence.Frame>
    {
        [Serializable]
        public struct Frame
        {
            public int frame;
            public int[] events;
        }

        [SerializeField]
        private Frame[] _frames;
        public Frame this[int i] => _frames[i];

        [SerializeField]
        private uint _animatedOn;
        public uint AnimatedOn => _animatedOn;

        public int Count => _frames.Length;

        public IEnumerator<Frame> GetEnumerator()
        {
            foreach (var f in _frames)
                yield return f;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _frames.GetEnumerator();
        }
    }
}
