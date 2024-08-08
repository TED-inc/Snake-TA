using UnityEngine;
using TEDinc.SnakeTA.IndependentLogic;
using System;

namespace TEDinc.SnakeTA.Contexts
{
    internal sealed class ProjectContext : MonoBehaviour
    {
        private void Awake()
        {
            AllServices.AddProcessor(new DisposableServiceProcessor());
            AllServices.AddProcessor(new TickableServiceProcessor());
            AllServices.Register(new string[4]);
            AllServices.Register(new string('1', 2));
            AllServices.Register(new Ticker());
        }

        private void OnDestroy() => 
            AllServices.Clear();

        internal class Ticker : ITickable, IDisposable
        {
            public void Dispose()
            {
                Debug.Log($"bye!");
            }

            public void Tick()
            {
                Debug.Log($"{Time.frameCount} {Time.deltaTime}");
            }
        }
    }
}