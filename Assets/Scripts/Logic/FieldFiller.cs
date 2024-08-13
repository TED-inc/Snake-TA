using System;
using UnityEngine;
using Random = System.Random;

namespace TEDinc.SnakeTA.Logic
{
    internal sealed class FieldFiller : IDisposable
    {
        // note: this consts and pool can be moved to some serializable data holder
        private const int INIT_EATABLE_CELLS_COUNT = 2;
        private const int RANDOM_ADD_CELL_ATTEMPT_LIMIT = 10;
        private static readonly RandomPool<ICellable> _eatableRandomPool = new() { 
            { new SizeChanger(1), 1.5f },
            { new SizeChanger(-1), 0.5f },
            { new SpeedChanger(2f, 10f), 0.5f },
            { new SpeedChanger(0.5f, 10f), 0.3f },
            { new DirectionChanger(), 0.2f },
        };

        private readonly Field _field;
        private readonly Random _random = new();

        public FieldFiller(Field field)
        {
            _field = field;
            _field.CellSet += TryAddEatableCell;
        }

        private void TryAddEatableCell(Vector2Int pos, ICellable removed, ICellable added)
        {
            if (FieldUtils.IsEatableConsumed(removed, added))
                AddEatableCell();
        }

        public void Init()
        {
            for (int i = 0; i < INIT_EATABLE_CELLS_COUNT; i++)
                AddEatableCell();
        }

        private void AddEatableCell()
        {
            for (int i = 0; i < RANDOM_ADD_CELL_ATTEMPT_LIMIT; i++)
            {
                Vector2Int pos = new(_random.Next(_field.Size.x), _random.Next(_field.Size.y));
                if (_field[pos] is null)
                {
                    _field[pos] = _eatableRandomPool.Next();
                    return;
                }
            }

            for (int x = 0; x < _field.Size.x; x++)
                for (int y = 0; y < _field.Size.y; y++)
                {
                    Vector2Int pos = new(x, y);
                    if (_field[pos] is null)
                    {
                        _field[pos] = _eatableRandomPool.Next();
                        return;
                    }
                }
        }

        public void Dispose()
        {
            _field.CellSet -= TryAddEatableCell;
        }
    }
}
