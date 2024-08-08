using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class Field : IReadOnlyField
    {
        public bool IsGameEnd { get; private set; }
        public Vector2Int Size { get; private set; }

        private readonly List<ICellable> _cells = new();
        private readonly Dictionary<ICellable, int> _repeatedCells = new();
        private readonly Queue<IFieldAction> _actionsQueue = new();

        public ICellable this[Vector2Int pos]
        {
            get => IsValidPos(pos) ? _cells[PosToIndex(pos)] : throw CreatePosException(pos);
            set
            {
                if (!IsValidPos(pos))
                    throw CreatePosException(pos);

                ICellable removedCell = _cells[PosToIndex(pos)];
                ICellable addedCell = value;

                if (removedCell != null && --_repeatedCells[removedCell] <= 0)
                    _repeatedCells.Remove(removedCell);

                _cells[PosToIndex(pos)] = addedCell;

                if (addedCell != null && !_repeatedCells.TryAdd(addedCell, 1))
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
            _cells.AddRange(Enumerable.Repeat<ICellable>(null, Size.x * Size.y));
            _repeatedCells.Clear();
            IsGameEnd = false;
        }

        public bool IsValidPos(Vector2Int pos) =>
            pos.x >= 0 && pos.y >= 0 && pos.x < Size.x && pos.y < Size.y;

        private int PosToIndex(Vector2Int pos) => 
            pos.y * Size.x + pos.x;

        private Exception CreatePosException(Vector2Int pos) =>
            new ArgumentOutOfRangeException($"{pos} is not in {Size}");

        public void Tick(float deltaTime)
        {
            if (IsGameEnd)
                return;

            foreach (ICellable cell in _repeatedCells.Keys)
                _actionsQueue.Enqueue(cell.Tick(deltaTime));
            
            while (_actionsQueue.Count > 0)
            {
                IFieldAction action = _actionsQueue.Dequeue();

                while (action != null)
                {
                    // execute action only if executor cell is at field, or was not specified 
                    if (action.ExecutorCell == null || _repeatedCells.ContainsKey(action.ExecutorCell))
                        action.Execute(this);

                    action = action.Next;
                }
            }
        }

        public void EndGame() => 
            IsGameEnd = true;
    }

    public interface ICellable 
    {
        internal IFieldAction Tick(float deltaTime);
    }

    internal sealed class FieldActionSet : IFieldAction
    {
        private readonly Vector2Int _pos;
        private readonly ICellable _setCell;
        public IFieldAction Next { get; }
        public ICellable ExecutorCell { get; }

        public FieldActionSet(Vector2Int pos, ICellable setCell, ICellable executorCell, IFieldAction next = null)
        {
            _pos = pos;
            _setCell = setCell;
            ExecutorCell = executorCell;
            Next = next;
        }

        public void Execute(Field field) => 
            field[_pos] = _setCell;
    }

    internal sealed class FieldActionEndGame : IFieldAction
    {
        public IFieldAction Next => null;

        public ICellable ExecutorCell => null;

        public void Execute(Field field) =>
            field.EndGame();
    }

    internal interface IFieldAction
    {
        public IFieldAction Next { get; }
        public ICellable ExecutorCell { get; }
        public void Execute(Field field);
    }

    public interface IReadOnlyField
    {
        public Vector2Int Size { get; }
        public ICellable this[Vector2Int pos] { get; }
    }
}
