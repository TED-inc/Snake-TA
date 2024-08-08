using TEDinc.SnakeTA.IndependentLogic;
using TEDinc.SnakeTA.Logic;
using UnityEngine;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldDisplayCell : MonoBehaviour, ITickable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _snakeColor = Color.white;
        [SerializeField] private Color _emptyColor = Color.gray;
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
            _spriteRenderer.color = cell is Snake ? _snakeColor : _emptyColor;
        }
    }
}
