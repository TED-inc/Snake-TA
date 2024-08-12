using TEDinc.SnakeTA.IndependentLogic;
using TEDinc.SnakeTA.Logic;
using UnityEngine;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldDisplayCell : MonoBehaviour, ITickable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _snakeColor = Color.white;
        [SerializeField] private Color _positiveSizeChangerColor = Color.green;
        [SerializeField] private Color _negativeSizeChangerColor = Color.red;
        [SerializeField] private Color _positiveSpeedChangerColor = Color.cyan;
        [SerializeField] private Color _negativeSpeedChangerColor = Color.magenta;
        [SerializeField] private Color _emptyColor = Color.gray;
        [SerializeField] private Color _otherColor = Color.black;
        private IReadOnlyField _field;
        private Vector2Int _pos;

        public void Setup(Vector2Int pos, IReadOnlyField field)
        {
            _field = field;
            _pos = pos;
            transform.localPosition = (Vector2)_pos;
        }

        public void Tick()
        {
            ICellable cell = _field[_pos];
            _spriteRenderer.color = GetColorOfCell(cell);
        }

        Color GetColorOfCell(ICellable cell)
        {
            if (cell is Snake)
                return _snakeColor;
            if (cell is SizeChanger sizeChanger)
                return sizeChanger.Value > 0f ? _positiveSizeChangerColor : _negativeSizeChangerColor;
            if (cell is SpeedChanger speedChanger)
                return speedChanger.Multiplicator > 1f ? _positiveSpeedChangerColor : _negativeSpeedChangerColor;
            if (cell == null)
                return _emptyColor;
            return _otherColor;
        }
    }
}
