using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public static class FieldUtils
    {
        public static Vector2Int LoopPos(Vector2Int pos, Vector2Int size) =>
            new(LoopLength(pos.x, size.x), LoopLength(pos.y, size.y));

        public static int LoopLength(int val, int length) =>
            (val % length + length) % length;
    }
}
