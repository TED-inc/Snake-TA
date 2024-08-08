using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.SnakeTA.IndependentLogic
{
    public sealed class TickableServiceProcessor : IServicesProcessor
    {
        public IReadOnlyList<ITickable> Tickables => _tickables;
        private readonly List<ITickable> _tickables = new();

        public TickableServiceProcessor() => 
            TickableServiceProcessorRunner.CreateRunner(this);

        public void Registered(object service)
        {
            if (service is ITickable tickable)
                _tickables.Add(tickable);
        }

        public void UnRegistered(object service)
        {
            if (service is ITickable tickable)
                _tickables.Remove(tickable);
        }
    }

    public interface ITickable
    {
        public void Tick();
    }

    internal sealed class TickableServiceProcessorRunner : MonoBehaviour
    {
        private TickableServiceProcessor _processor;

        private void Update()
        {
            foreach (ITickable tickable in _processor.Tickables)
                tickable.Tick();
        }

        public static void CreateRunner(TickableServiceProcessor processor)
        {
            GameObject go = new ("[" + nameof(TickableServiceProcessorRunner) + "]");
            DontDestroyOnLoad(go);
            go.AddComponent<TickableServiceProcessorRunner>()._processor = processor;
        }
    }
}
