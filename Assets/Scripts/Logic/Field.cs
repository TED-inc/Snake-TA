using System;
using System.Collections.Generic;
using System.Linq;
using TEDinc.SnakeTA.IndependentLogic;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class Field : ITickable
    {
        public Vector2Int Size { get; private set; }

        private readonly List<ICell> _cells = new();
        private readonly Dictionary<ICell, int> _repeatedCells = new();

        public ICell this[Vector2Int pos]
        {
            get => IsValidPos(pos) ? _cells[PosToIndex(pos)] : throw CreatePosException(pos);
            set
            {
                if (!IsValidPos(pos))
                    throw CreatePosException(pos);

                ICell removedCell = _cells[PosToIndex(pos)];
                ICell addedCell = value;

                if (removedCell != null && --_repeatedCells[removedCell] <= 0)
                    _repeatedCells.Remove(removedCell);

                _cells[PosToIndex(pos)] = addedCell;

                if (!_repeatedCells.TryAdd(addedCell, 1))
                    _repeatedCells[addedCell]++;
            }
        }

        public void SetSizeAndClear(Vector2Int size)
        {
            Size = size;
            Clear();
        }

        private void Clear()
        {
            _cells.Clear();
            _cells.AddRange(Enumerable.Repeat<ICell>(null, Size.x * Size.y));
            _repeatedCells.Clear();
        }

        public bool IsValidPos(Vector2Int pos) =>
            pos.x >= 0 && pos.y >= 0 && pos.x < Size.x && pos.y < Size.y;

        private int PosToIndex(Vector2Int pos) => 
            pos.y * Size.x + pos.x;

        private Exception CreatePosException(Vector2Int pos) =>
            new ArgumentOutOfRangeException($"{pos} is not in {Size}");

        public void Tick()
        {
            foreach (ICell cell in _repeatedCells.Keys)
            {
                IFieldAction action = cell.Tick();
                while (action != null)
                {
                    action.Execute(this);
                    action = action.Next;
                }
            }
        }
    }

    public interface ICell 
    {
        internal IFieldAction Tick();
    }

    internal sealed class FieldActionSet : IFieldAction
    {
        public readonly Vector2Int Pos;
        public readonly ICell Cell;
        public IFieldAction Next { get; }

        public FieldActionSet(Vector2Int pos, ICell cell, IFieldAction next = null)
        {
            Pos = pos;
            Cell = cell;
            Next = next;
        }

        public void Execute(Field field) => 
            field[Pos] = Cell;
    }

    internal interface IFieldAction
    {
        public IFieldAction Next { get; }
        public void Execute(Field field);
    }
}
