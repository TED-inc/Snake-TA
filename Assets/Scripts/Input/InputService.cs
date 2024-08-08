using System;
using TEDinc.SnakeTA.IndependentLogic;
using TEDinc.SnakeTA.Logic;
using UnityEngine;

namespace TEDinc.SnakeTA.Input
{
    public sealed class InputService : ITickable, IDisposable
    {
        private readonly FieldService _fieldService;
        private readonly PlayerInput _inputs;

        public InputService()
        {
            AllServices.Get(out _fieldService);
            _inputs = new();
            _inputs.Enable();
        }

        public void Tick()
        {
            Vector2 movement = _inputs.Main.Movement.ReadValue<Vector2>();
            _fieldService.Snake.TrySetDirection(new ((int)movement.x, (int)movement.y));
        }

        public void Dispose()
        {
            _inputs.Dispose();
        }
    }
}
