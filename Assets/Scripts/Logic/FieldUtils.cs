using System;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public static class FieldUtils
    {
        public static Vector2Int LoopPos(Vector2Int pos, Vector2Int size) =>
            new(LoopLength(pos.x, size.x), LoopLength(pos.y, size.y));

        public static int LoopLength(int val, int length) =>
            (val % length + length) % length;

        public static bool IsEatable(ICellable cellable) =>
            cellable is null ? 
                throw new ArgumentNullException() :
                cellable is not Snake;

        public static bool IsEatableConsumed(ICellable removed, ICellable added) =>
            removed is not null && IsEatable(removed) && added is Snake;
    }
}
