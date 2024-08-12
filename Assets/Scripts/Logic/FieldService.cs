using TEDinc.SnakeTA.IndependentLogic;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class FieldService : ITickable
    {
        public event Action OnStart;

        public float Speed { get; set; } = 2f;
        public Snake Snake { get; private set; }
        public IReadOnlyField Field => _field;

        private readonly Field _field = new();

        public void StartGame()
        {
            _field.SetSizeAndClear(new(10, 6));
            Vector2Int[] snakeBody = new Vector2Int[] { new(4, 2), new(5, 2), new(6, 2), new(6, 3), new(6, 4) };
            Snake = new(snakeBody, _field);
            foreach (Vector2Int bodyPos in snakeBody)
                _field[bodyPos] = Snake;
            Snake.TrySetDirection(Vector2Int.left);

            _field[new(1, 2)] = new SizeChanger(1);
            _field[new(7, 2)] = new SizeChanger(10);
            _field[new(4, 4)] = new SizeChanger(-1);
            _field[new(8, 4)] = new SizeChanger(-10);
            _field[new(2, 3)] = new SpeedChanger(2f, 5f);
            _field[new(8, 3)] = new SpeedChanger(2f, 5f);
            _field[new(2, 5)] = new SpeedChanger(0.5f, 5f);
            _field[new(8, 5)] = new SpeedChanger(0.5f, 5f);
            _field[new(0, 2)] = new DirectionChanger();

            OnStart?.Invoke();
        }

        public void Tick() => 
            _field.Tick(Time.deltaTime * Speed);
    }
}
