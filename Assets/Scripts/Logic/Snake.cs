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
        // NOTE: in case of expansion of effect system / more temporary effects - create proper system for handling it
        private readonly LinkedList<SpeedEffect> _speedEffects = new();
        private readonly IReadOnlyField _field;
        private Vector2Int _direction;
        private float _movementProgress;
        private float _speed = 1f;

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
            float multiplicator = GetSpeedEffectMultiplicatorAndTickEffect(deltaTime);

            _movementProgress += _speed * deltaTime * multiplicator;

            if (_movementProgress < 1f)
                return null;

            _movementProgress %= 1f;
            Vector2Int nextHeadPos = GetNextHeadPos();
            ICellable nextCell = _field[nextHeadPos];

            if (nextCell is Snake)
                return new FieldActionEndGame();

            if (nextCell is SpeedChanger speedChanger)
                _speedEffects.AddFirst(new SpeedEffect(speedChanger));

            IFieldAction action = CreateMoveAndSizeChangeFieldAction(nextHeadPos, nextCell);

            if (nextCell is DirectionChanger)
            {
                ReverseRecoursive(_body);
                TrySetDirection(_direction * -Vector2Int.one);
            }

            return action;
        }

        private float GetSpeedEffectMultiplicatorAndTickEffect(float deltaTime)
        {
            LinkedListNode<SpeedEffect> speedEffectNode = _speedEffects.First;
            float multiplicator = 1f;

            while (speedEffectNode != null)
            {
                LinkedListNode<SpeedEffect> nextNode = speedEffectNode.Next;
                SpeedEffect speedEffect = speedEffectNode.Value;
                speedEffect.Duration -= deltaTime;

                if (speedEffect.Duration < 0f)
                    _speedEffects.Remove(speedEffectNode);
                else
                {
                    multiplicator *= speedEffect.Multiplicator;
                    speedEffectNode.Value = speedEffect;
                }

                speedEffectNode = nextNode;
            }

            return multiplicator;
        }

        private IFieldAction CreateMoveAndSizeChangeFieldAction(Vector2Int nextHeadPos, ICellable nextCell)
        {
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

            return new FieldActionSet(pos: nextHeadPos, setCell: this, executorCell: this, next: action);
        }

        private Vector2Int GetNextHeadPos() =>
            GetNextHeadPos(_direction);

        private Vector2Int GetNextHeadPos(Vector2Int direction) =>
            FieldUtils.LoopPos(_body.First.Value + direction, _field.Size);

        private struct SpeedEffect
        {
            public readonly float Multiplicator;
            public float Duration;

            public SpeedEffect(SpeedChanger speedChanger)
            {
                Multiplicator = speedChanger.Multiplicator;
                Duration = speedChanger.Duration;
            }
        }

        private static void ReverseRecoursive<T>(LinkedList<T> list)
        {
            if (list.Count <= 1)
                return;

            LinkedListNode<T> node = list.First;
            list.RemoveFirst();
            ReverseRecoursive(list);
            list.AddLast(node);
        }
    }
}
