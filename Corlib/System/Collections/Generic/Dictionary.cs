
namespace System.Collections.Generic
{
    /*
     public class Dictionary<TKey, TValue> where TKey : IEquatable<TKey>
     {
         const int InitialCapacity = 11;
         const double MaxLoadFactor = 0.75;
         List<KeyValuePair<TKey, TValue>>[] container;
         double LoadFactor { get => Count / Capacity; }
         public int Count { get; private set; }
         public int Capacity { get; private set; }
         public Dictionary(int capacity = InitialCapacity)
         {
             Capacity = capacity;
             container = new List<KeyValuePair<TKey, TValue>>[capacity];
         }
         int GetIndex(TKey key)
         {
             if (key is null)
             {
                 throw new ArgumentNullException("key is null");
             }
             return ( key.GetHashCode() % Capacity + Capacity ) % Capacity;
         }
         int FindListIndexByKey(List<KeyValuePair<TKey, TValue>> list, TKey key) =>
             list is null
             ? -1
             : list.FindIndex(pair => pair.Key.Equals(key));
         //^ find index is broken
         int FindListIndexByValue(List<KeyValuePair<TKey, TValue>> list, TValue value) =>
             list is null
             ? -1
             : list.FindIndex(pair => pair.Value.Equals(value));
         //^ find index is broken
         TValue GetValue(TKey key)
         {
             var index = GetIndex(key);
             var list = container[index];
             var listIndex = FindListIndexByKey(list, key);
             if (listIndex < 0)
             {
                 throw new KeyNotFoundException("key doesn`t exist");
                 //^ KeyNotFoundException is broken
             }
             return list[listIndex].Value;
         }
         int TryGetListIndexOfEntry(TKey key, out List<KeyValuePair<TKey, TValue>> list)
         {
             int index = GetIndex(key);
             list = container[index]
                 ??= new List<KeyValuePair<TKey, TValue>>();
             return FindListIndexByKey(list, key);
         }
         void SetValue(TKey key, TValue value, bool allowModifyExisting = false)
         {
             var pair = new KeyValuePair<TKey, TValue>(key, value);
             if (!ContainsKey(key))
             {
                 if ((double)( Count + 1 ) / Capacity > MaxLoadFactor)
                 {
                     Resize();
                 }
                 var listIndex = TryGetListIndexOfEntry(key, out var list);
                 list.Add(pair);
                 ++Count;
             }
             else if (allowModifyExisting)
             {
                 var listIndex = TryGetListIndexOfEntry(key, out var list);
                 list[listIndex] = pair;
             }
             else
             {
                 throw new ArgumentException("key already exists");
             }
         }
         public bool ContainsKey(TKey key)
         {
             int containerIndex = GetIndex(key);
             var listIndex = FindListIndexByKey(container[containerIndex], key);
             return listIndex >= 0;
         }
         public bool ContainsValue(TValue value)
         {
             int containerIndex = 0;
             int listIndex = -1;
             while (containerIndex < container.Length)
             {
                 listIndex = FindListIndexByValue(container[containerIndex], value);
                 ++containerIndex;
             }
             return listIndex >= 0;
         }
         public bool TryGetValue(TKey key, out TValue value)
         {
             var index = GetIndex(key);
             var list = container[index];
             var listIndex = FindListIndexByKey(list, key);
             var hasEntry = listIndex >= 0;
             value = hasEntry ? default : list[listIndex].Value;
             return hasEntry;
         }
         public void Add(TKey key, TValue value) => SetValue(key, value);
         public bool Remove(TKey key)
         {
             var index = GetIndex(key);
             var list = container[index];
             var listIndex = FindListIndexByKey(list, key);
             var hasEntry = listIndex >= 0;
             if (hasEntry)
             {
                 list.RemoveAt(listIndex);
                 --Count;
             }
             return hasEntry;
         }
         public void Clear()
         {
             foreach (var list in container)
             {
                 list.Clear();
             }
             Count = 0;
         }
         public TValue this[TKey key]
         {
             get => GetValue(key);
             set => SetValue(key, value, true);
         }
         void Resize()
         {
             var oldContainer = container;
             container = new List<KeyValuePair<TKey, TValue>>[Capacity *= 2];
             foreach (var list in oldContainer)
             {
                 if (list is null) continue;
                 foreach (var pair in list)
                 {
                     this[pair.Key] = pair.Value;
                 }
             }
         }
         public IEnumerable<KeyValuePair<TKey, TValue>> getAll()
         {
             var result = new List<KeyValuePair<TKey, TValue>>(Count);
             foreach (var list in container)
             {
                 if (list is null) continue;
                 result.AddRange(list);
                 //^ AddRange is broken
             }
             return result;
         }
     }
     */
}