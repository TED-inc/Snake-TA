using TEDinc.SnakeTA.IndependentLogic;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class FieldService : ITickable
    {
        public event Action OnStart;
        public readonly Field Field = new();
        public Snake Snake { get; private set; }

        public void StartGame()
        {
            Field.SetSizeAndClear(new(10, 6));
            Vector2Int[] snakeBody = new Vector2Int[] { new(4, 2), new(5, 2), new(6, 2) };
            Snake = new(snakeBody, Field.Size);
            foreach (Vector2Int bodyPos in snakeBody)
                Field[bodyPos] = Snake;
            Snake.TrySetDirection(Vector2Int.left);

            OnStart?.Invoke();
        }

        public void Tick()
        {
            Field.Tick(Time.deltaTime);
        }
    }
}
