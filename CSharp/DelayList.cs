using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Altseed
{
    [Serializable]
    internal class DelayList<T> : IReadOnlyCollection<T>
    {
        private readonly List<T> objects;
        private readonly List<T> waitingAdded = new List<T>();
        private readonly List<T> waitingRemoved = new List<T>();

        public event Action<T> OnAdded = delegate { };
        public event Action<T> OnRemoved = delegate { };

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

        public void Add(T item) => waitingAdded.Add(item);
        public void Remove(T item) => waitingRemoved.Add(item);
        public bool Contains(T item) => objects.Contains(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            foreach(var x in waitingAdded)
            {
                objects.Add(x);
                OnAdded(x);
            }

            foreach(var x in waitingRemoved)
            {
                _ = objects.Remove(x);
                OnRemoved(x);
            }

            waitingAdded.Clear();
            waitingRemoved.Clear();
        }

        public int Count => objects.Count;

        public IEnumerator<T> GetEnumerator() => objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}