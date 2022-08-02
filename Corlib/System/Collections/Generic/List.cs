using Internal.Runtime.CompilerServices;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	public class List<T>
	{
		private T[] _value;
		private int size;
		public int Count = 0;

		public List(int initsize = 256)
		{
			size = initsize;
			_value = new T[initsize];
		}
		public List(T[] array)
		{
			size = array.Length;
			_value = array;
		}
		public T this[int index]
		{
			get => _value[index];
			set => _value[index] = value;
		}

		public void Add(T t)
		{
			if (size == Count)
			{
				Array.Resize(ref _value, size + 1);
				size++;
			}
			_value[Count] = t;
			Count++;
		}

		public void Insert(int index, T item, bool internalMove = false)
		{
			if (index == IndexOf(item))
			{
				return;
			}

			if (!internalMove)
			{
				Count++;
			}

			if (internalMove)
			{
				int _index = IndexOf(item);
				for (int i = _index; i < Count - 1; i++)
				{
					_value[i] = _value[i + 1];
				}
			}

			for (int i = Count - 1; i > index; i--)
			{
				_value[i] = _value[i - 1];
			}
			_value[index] = item;
		}

		public T[] ToArray()
		{
			T[] array = new T[Count];
			for (int i = 0; i < Count; i++)
			{
				array[i] = this[i];
			}
			return array;
		}

        public int IndexOf(T item)
		{
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == item)
				{
					return i;
				}
			}

			return -1;
		}
		public bool Remove(T item)
		{
			int at = IndexOf(item);

			if (at < 0)
			{
				return false;
			}

			RemoveAt(at);

			return true;
		}

		public void RemoveAt(int index)
		{
			Count--;

			for (int i = index; i < Count; i++)
			{
				_value[i] = _value[i + 1];
			}

			_value[Count] = default;
		}

		public override void Dispose()
		{
			_value.Dispose();
			base.Dispose();
		}

		public void Clear()
		{
			Count = 0;
		}

	}
}