using UnityEngine;
using TEDinc.SnakeTA.IndependentLogic;
using System;
using TEDinc.SnakeTA.Logic;

namespace TEDinc.SnakeTA.Contexts
{
    [DefaultExecutionOrder(-1000)]
    internal sealed class ProjectContext : MonoBehaviour
    {
        private void Awake()
        {
            AllServices.AddProcessor(new DisposableServiceProcessor());
            AllServices.AddProcessor(new TickableServiceProcessor());
            Field field = new ();
            field.SetSizeAndClear(new (10, 6));
            Vector2Int[] snakeBody = new Vector2Int[] { new (4, 2), new (5, 2), new (6, 2) };
            Snake snake = new (snakeBody, field.Size);
            foreach (Vector2Int bodyPos in snakeBody)
                field[bodyPos] = snake;
            snake.TrySetDirection(Vector2Int.left);
            AllServices.Register(field);
        }

        private void OnDestroy() => 
            AllServices.Clear();
    }
}