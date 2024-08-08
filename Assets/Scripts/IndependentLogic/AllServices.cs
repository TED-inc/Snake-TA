using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.SnakeTA
{
    public sealed class AllServices
    {
        public event Action<object> OnServiceAdded;
        public event Action<object> OnServiceRemoved;
        
        private static AllServices _instance;
        private readonly Dictionary<Type, object> _container = new();
        private readonly Dictionary<Type, IServicesProcessor> _processors = new();
        private readonly LinkedList<object> _removables = new();

        public static void Clear()
        {
            if (_instance == null)
                return;

            LinkedListNode<object> lastNode = _instance._removables.Last;
            while (lastNode != null)
            {
                object removable = lastNode.Value;
                lastNode = lastNode.Previous;

                if (removable is IServicesProcessor)
                    RemoveProcessor(removable.GetType());
                else
                    UnRegister(removable.GetType());
            }
        }

        public static void AddProcessor(IServicesProcessor processor)
        {
            _instance ??= new AllServices();

            if (_instance._processors.TryAdd(processor.GetType(), processor))
                _instance._removables.AddLast(processor);
        }

        public static void RemoveProcessor<TProcessor>() where TProcessor : IServicesProcessor => 
            RemoveProcessor(typeof(TProcessor));

        private static void RemoveProcessor(Type type)
        {
            _instance ??= new AllServices();

            if (_instance._processors.Remove(type, out IServicesProcessor processor))
                _instance._removables.Remove(processor);
        }

        public static void Register<TService>(TService implementation) where TService : class
        {
            _instance ??= new AllServices();

            if (_instance._container.TryAdd(typeof(TService), implementation))
            {
                foreach (IServicesProcessor processor in _instance._processors.Values)
                    processor.Registered(implementation);

                _instance._removables.AddLast(implementation);
                _instance.OnServiceAdded?.Invoke(implementation);
                return;
            }
            Debug.LogWarning($"Failed to add service of type {typeof(TService)}!");
        }
        public static void UnRegister<TService>() where TService : class =>
            UnRegister(typeof(TService));

        private static void UnRegister(Type type)
        {
            _instance ??= new AllServices();

            if (_instance._container.Remove(type, out object implementation))
            {
                foreach (IServicesProcessor processor in _instance._processors.Values)
                    processor.UnRegistered(implementation);

                _instance._removables.Remove(implementation);
                _instance.OnServiceRemoved?.Invoke(implementation);
                return;
            }
            Debug.LogWarning($"Failed to remove service of type {type}!");
        }

        public static TService Get<TService>() where TService : class
        {
            if (_instance._container.TryGetValue(typeof(TService), out object implementation))
                return implementation as TService;

            throw new Exception($"Failed to get service of type {typeof(TService)}!");
        }
    }

    public interface IServicesProcessor
    {
        void Registered(object service);
        void UnRegistered(object service);
    }
}