using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class RandomPool<T> : IEnumerable<KeyValuePair<T, float>>
    {
        private readonly List<(T value, float weight, float commulativeWeight)> _pool = new();
        private readonly Random _random = new();

        private float TotalWeight => _pool.Count > 0 ? _pool[^1].commulativeWeight : 0f;

        public void Add(T value, float weight)
        {
            if (weight < 0f)
                throw new ArgumentOutOfRangeException();

            float commulativeWeight = TotalWeight + weight;
            _pool.Add((value, weight, commulativeWeight));
        }

        public T Next()
        {
            float targetWeight = (float)_random.NextDouble() * TotalWeight;
            int index = _pool.Count / 2;
            int delta = _pool.Count / 4;
            while (true)
            {
                int comparison = GetBinaryComparison(index);
                if (comparison == 0)
                    return _pool[index].value;

                index += delta * comparison;
                delta = Math.Max(1, delta / 2);
            }

            throw new SystemException();
            
            
            int GetBinaryComparison(int index)
            {
                (T value, float weight, float commulativeWeight) = _pool[index];
                float fromWeight = commulativeWeight - weight;
                float toWeight = commulativeWeight;

                if (targetWeight < fromWeight)
                    return -1;
                if (targetWeight >= toWeight)
                    return 1;
                return 0;
            }
        }

        public IEnumerator<KeyValuePair<T, float>> GetEnumerator()
        {
            foreach ((T value, float weight, float commulativeWeight) in _pool)
                yield return new (value, weight);
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
