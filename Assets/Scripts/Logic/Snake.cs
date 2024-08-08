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
        private readonly Vector2Int _fieldSize;
        private Vector2Int _direction;
        private float _movementProgress;
        private float _speed = 1;

        public Snake(IEnumerable<Vector2Int> body, Vector2Int fieldSize)
        {
            _body = new (body);
            _fieldSize = fieldSize;
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
            Vector2Int nextHead = GetNextHeadPos();
            IFieldAction action = new FieldActionSet(nextHead, this);
            _body.AddFirst(nextHead);

            if (_body.Count > 1)
            {
                Vector2Int prevTail = _body.Last.Value;
                _body.RemoveLast();
                action = new FieldActionSet(prevTail, null, action);
            }

            return action;
        }

        private Vector2Int GetNextHeadPos() =>
            GetNextHeadPos(_direction);

        private Vector2Int GetNextHeadPos(Vector2Int direction) =>
            FieldUtils.LoopPos(_body.First.Value + direction, _fieldSize);
    }
}
