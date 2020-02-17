using System;
using System.Collections;
using System.Collections.Generic;

namespace Altseed
{
    public interface IDelayHashSet<T> : IReadOnlyCollection<T>
    {
        void Add(T item);
        void Remove(T item);
        bool Contains(T item);
    }

    [Serializable]
    public class DelayHashSet<T> : IDelayHashSet<T>
    {
        [Serializable]
        private enum Waiting
        {
            Add,
            Remove,
        }

        private readonly HashSet<T> objects = new HashSet<T>();
        private readonly Dictionary<T, Waiting> waitings = new Dictionary<T, Waiting>();

        public DelayHashSet() { }

        public void Add(T item) => waitings[item] = Waiting.Add;
        public void Remove(T item) => waitings[item] = Waiting.Remove;
        public bool Contains(T item) => objects.Contains(item);

        public void Update()
        {
            foreach(var x in waitings)
            {
                switch(x.Value)
                {
                    case Waiting.Add:
                        objects.Add(x.Key);
                        break;
                    case Waiting.Remove:
                        objects.Remove(x.Key);
                        break;
                    default:
                        break;
                };
            }

            waitings.Clear();
        }

        public int Count => objects.Count;

        public IEnumerator<T> GetEnumerator() => objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}