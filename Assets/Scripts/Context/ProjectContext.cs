using UnityEngine;
using TEDinc.SnakeTA.IndependentLogic;
using System;
using TEDinc.SnakeTA.Logic;
using TEDinc.SnakeTA.Input;

namespace TEDinc.SnakeTA.Contexts
{
    [DefaultExecutionOrder(-1000)]
    internal sealed class ProjectContext : MonoBehaviour
    {
        private void Awake()
        {
            AllServices.AddProcessor(new DisposableServiceProcessor());
            AllServices.AddProcessor(new TickableServiceProcessor());
            
            AllServices.Register(new FieldService());
            AllServices.Register(new InputService());
        }

        private void Start() => 
            AllServices.Get<FieldService>().StartGame();

        private void OnDestroy() => 
            AllServices.Clear();
    }
}