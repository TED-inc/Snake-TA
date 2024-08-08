using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class Snake : ICellable
    {
        public Vector2Int Direction => _direction;
        public IReadOnlyCollection<Vector2Int> Body => _body;

        private readonly LinkedList<Vector2Int> _body;
        private readonly IReadOnlyField _field;
        private Vector2Int _direction;
        private float _movementProgress;
        private float _speed = 1;

        public Snake(IEnumerable<Vector2Int> body, IReadOnlyField field)
        {
            _body = new (body);
            _field = field;
        }

        public bool TrySetDirection(Vector2Int direction)
        {
            if (direction.sqrMagnitude != 1)
                return false;

            if (_body.Count > 1 && GetNextHeadPos(direction) == _body.ElementAt(1))
                return false;

            _direction = direction;
            return true;
        }

        IFieldAction ICellable.Tick(float deltaTime)
        {
            _movementProgress += _speed * deltaTime;

            if (_movementProgress < 1f)
                return null;

            _movementProgress %= 1f;
            Vector2Int nextHeadPos = GetNextHeadPos();
            ICellable nextCell = _field[nextHeadPos];

            if (nextCell is Snake)
                return new FieldActionEndGame();

            IFieldAction action = null;
            _body.AddFirst(nextHeadPos);

            int sizeChange = (nextCell as SizeChanger)?.Value ?? 0;
            int trimTail = Mathf.Clamp(1 - sizeChange, 0, _body.Count - 1);
            for (int i = 0; i < trimTail; i++)
            {
                Vector2Int prevTailPos = _body.Last.Value;
                _body.RemoveLast();
                action = new FieldActionSet(pos: prevTailPos, setCell: null, executorCell: this, next: action);
            }

            action = new FieldActionSet(pos: nextHeadPos, setCell: this, executorCell: this, next: action);

            return action;
        }

        private Vector2Int GetNextHeadPos() =>
            GetNextHeadPos(_direction);

        private Vector2Int GetNextHeadPos(Vector2Int direction) =>
            FieldUtils.LoopPos(_body.First.Value + direction, _field.Size);
    }
}
