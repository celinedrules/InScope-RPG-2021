using System.Collections;
using System.Collections.Generic;
using Items;

namespace UI
{
    public class Page : IList<Item>
    {
        private readonly List<Item> items = new List<Item>();

        public int Count => items.Count;
        public bool IsReadOnly => false;
        public Item this[int index] { get => items[index]; set => items[index] = value; }
    
        public IEnumerator<Item> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(Item item) => items.Add(item);
        public void Clear() => items.Clear();
        public bool Contains(Item item) => items.Contains(item);
        public void CopyTo(Item[] array, int arrayIndex) => ((IList<Item>) this).CopyTo(array, arrayIndex);
        public bool Remove(Item item) => items.Remove(item);
        public int IndexOf(Item item) => items.IndexOf(item);
        public void Insert(int index, Item item) => items.Insert(index, item);
        public void RemoveAt(int index) => items.RemoveAt(index);
    }
}