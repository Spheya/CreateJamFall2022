using UnityEngine;

namespace FlipAnimator
{
    [RequireComponent(typeof(MeshRenderer))]
    public class FlipAnimationPlayer : MonoBehaviour
    {
        [SerializeField]
        private FlipAnimation _animation;
        public FlipAnimation Animation {
            get => _animation;
            set {
                if (_animation != null)
                {
                    _animation.OnFrameUpdate -= UpdateFrame;
                    FlipAnimator.StopAnimation(_animation);
                }

                _animation = value;
                _animation.OnFrameUpdate += UpdateFrame;
                FlipAnimator.StartAnimation(_animation);
                UpdateFrame();
            }
        }

        [SerializeField]
        private Vector2Int _cellSize = new Vector2Int(32, 32);
        public Vector2Int CellSize { get => _cellSize; set => _cellSize = value; }

        private int _cellsPerRow;
        private Vector2 _uvScale;

        private Material _material;

        public FlipAnimationSequence.Frame? CurrentFrame => _animation == null ? null : new FlipAnimationSequence.Frame?(_animation.Frame);

        public delegate void FrameUpdateEvent(FlipAnimationSequence.Frame frame);
        public event FrameUpdateEvent OnFrameUpdate;

        void OnEnable()
        {
            _material = GetComponent<MeshRenderer>().material;
            var texture = _material.mainTexture;

            _uvScale = new Vector2((float)_cellSize.x / texture.width, (float) _cellSize.y / texture.height);
            _cellsPerRow = texture.width / _cellSize.x;

            if (_animation != null)
            {
                _animation.OnFrameUpdate += UpdateFrame;
                FlipAnimator.StartAnimation(_animation);
                UpdateFrame();
            }
        }

        void OnDisable()
        {
            if (_animation != null)
            {
                _animation.OnFrameUpdate -= UpdateFrame;
                FlipAnimator.StopAnimation(_animation);
            }
        }

        private void UpdateFrame()
        {
            int x = _animation.Frame.frame % _cellsPerRow;
            int y = _animation.Frame.frame / _cellsPerRow;

            _material.mainTextureOffset = _uvScale * new Vector2(x, y);
            _material.mainTextureScale = _uvScale;

            OnFrameUpdate?.Invoke(_animation.Frame);
        }
    }
}