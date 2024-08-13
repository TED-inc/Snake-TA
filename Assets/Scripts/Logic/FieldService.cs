using TEDinc.SnakeTA.IndependentLogic;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEditorInternal;
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
        private readonly FieldFiller _fieldFiller;

        public FieldService() => 
            _fieldFiller = new(_field);

        public void StartGame()
        {
            // note: this magic numbers can be moved to some serializable data holder
            _field.SetSizeAndClear(new(10, 6));
            Vector2Int[] snakeBody = new Vector2Int[] { new(4, 2), new(5, 2), new(6, 2), new(6, 3), new(6, 4) };

            Snake = new(snakeBody, _field);
            foreach (Vector2Int bodyPos in snakeBody)
                _field[bodyPos] = Snake;
            Snake.TrySetDirection(Vector2Int.left);

            _fieldFiller.Init();

            OnStart?.Invoke();
        }

        public void Tick() => 
            _field.Tick(Time.deltaTime * Speed);
    }
}
