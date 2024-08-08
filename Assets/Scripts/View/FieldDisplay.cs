using UnityEngine;
using TEDinc.SnakeTA.Logic;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldDisplay : MonoBehaviour
    {
        private Field _field;

        private void Awake()
        {
            AllServices.Get(out _field);
        }

        private void OnDrawGizmos()
        {
            if (_field == null)
                return;

            for (int x = 0; x < _field.Size.x; x++)
                for (int y = 0; y < _field.Size.y; y++)
                {
                    Vector2Int pos = new(x, y);
                    ICellable cell = _field[pos];
                    Gizmos.color = cell is Snake ? Color.white : Color.gray;
                    if (cell != null)
                        Gizmos.DrawCube((Vector2)pos, Vector2.one * 0.95f);
                    else
                        Gizmos.DrawWireCube((Vector2)pos, Vector2.one * 0.95f);
                }
        }
    }
}
