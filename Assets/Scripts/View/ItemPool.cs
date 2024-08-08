using System;
using System.Collections.Generic;
using TEDinc.SnakeTA.Logic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TEDinc.SnakeTA.View
{
    [Serializable]
    internal sealed class ItemPool<T> where T : Component
    {
        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private T _prototype;
        private readonly List<T> _pool = new();
        private int _nextIndex = 0;
        public int ActiveCount { get; private set; }
        public Transform Parent => _parent;

        public ItemPool() { }

        public ItemPool(T prototype, Transform parent)
        {
            _parent = parent;
            _prototype = prototype;
        }

        public T Next()
        {
            while (_pool.Count <= _nextIndex)
            {
                _pool.Add(Object.Instantiate(_prototype, _parent));
                _pool[^1].gameObject.SetActive(true);
            }

            T item = _pool[_nextIndex++];
            item.gameObject.SetActive(true);
            ActiveCount = _nextIndex;

            return item;
        }

        public void Clear()
        {
            _nextIndex = 0;
            ActiveCount = 0;
            FinishIterating();
        }

        public void FinishIterating()
        {
            for (int i = _nextIndex; i < _pool.Count; i++)
                _pool[i].gameObject.SetActive(false);
            _nextIndex = 0;
        }

        public void ReleaseLast()
        {
            if (_nextIndex == 0)
                throw new InvalidOperationException();

            _nextIndex--;
            ActiveCount--;
            _pool[_nextIndex].gameObject.SetActive(false);
        }

        public void ReleaseAt(int index)
        {
            if (index >= ActiveCount)
                throw new InvalidOperationException();

            _nextIndex--;
            ActiveCount--;
            T item = _pool[index];
            _pool.RemoveAt(index);
            _pool.Add(item);
            item.gameObject.SetActive(false);
        }

        public int GetIndex(T obj)
        {
            return _pool.FindIndex(item => item.Equals(obj));
        }

        public IEnumerable<T> EnumerateActiveItems()
        {
            for (int i = 0; i < ActiveCount; i++)
                yield return _pool[i];
        }
    }
}
