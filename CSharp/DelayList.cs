using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Altseed
{
    [Serializable]
    internal class DelayList<T> : IReadOnlyCollection<T>
    {
        [Serializable]
        private enum Waiting
        {
            Add,
            Remove,
        }

        private readonly List<T> objects;
        private readonly Dictionary<T, Waiting> waitings = new Dictionary<T, Waiting>();

        public DelayList()
        {
            objects = new List<T>();
        }

        public DelayList(int capacity)
        {
            objects = new List<T>(capacity);
        }

        public DelayList(IEnumerable<T> collection)
        {
            objects = new List<T>(collection);
        }

        public void Add(T item) => waitings[item] = Waiting.Add;
        public void Remove(T item) => waitings[item] = Waiting.Remove;
        public bool Contains(T item) => objects.Contains(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            foreach(var x in waitings)
            {
                switch(x.Value)
                {
                    case Waiting.Add:
                        if(!objects.Contains(x.Key))
                        {
                            objects.Add(x.Key);
                        }
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