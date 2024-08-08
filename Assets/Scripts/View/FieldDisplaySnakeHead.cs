using System.Linq;
using TEDinc.SnakeTA.IndependentLogic;
using TEDinc.SnakeTA.Logic;
using UnityEngine;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldDisplaySnakeHead : MonoBehaviour, ITickable
    {
        [SerializeField] private Transform _head;
        private Snake _snake;

        public void Setup(Snake snake) => 
            _snake = snake;

        public void Tick()
        {
            _head.localPosition = (Vector2)_snake.Body.First();
            _head.right = (Vector2)_snake.Direction;
        }
    }
}
