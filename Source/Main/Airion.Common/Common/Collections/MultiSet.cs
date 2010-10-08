// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public class MultiSet<T> : CollectionBase<T>
	{
		#region Nested Classes (1)
		
		private class MultiSetEnumerator : IEnumerator<T>
		{
			#region Constructors (1)

			public MultiSetEnumerator(MultiSet<T> multiset)
			{
				_multiset = multiset;
				_originalVersion = multiset._version;
				_itemEnumerator = multiset._items.Keys.GetEnumerator();
				_itemIndex = -1;
				_reachedEnd = false;
			}

			#endregion Constructors

			#region Methods (4)

			// Public Methods (3)

			public void Dispose()
			{
				_itemIndex = -1;
				_reachedEnd = false;
				_itemEnumerator.Dispose();
			}

			public bool MoveNext()
			{
				CheckVersion();
				bool isValid;
				if(_reachedEnd) {
					// special case for when we've already reached the end.
					isValid = false;
				} else if(_itemIndex == -1) {
					// special case for first item
					if(_itemEnumerator.MoveNext()) {
						_itemIndex = 0;
						isValid = true;
					} else {
						_reachedEnd = true;
						isValid = false;
					}
				} else {
					int lastItemIndex = _multiset._items[_itemEnumerator.Current] - 1;
					if(_itemIndex == lastItemIndex) {
						if(_itemEnumerator.MoveNext()) {
							_itemIndex = 0;
							isValid = true;
						} else {
							_reachedEnd = true;
							isValid = false;
						}
					} else {
						_itemIndex++;
						isValid = true;
					}
				}
				return isValid;
			}

			public void Reset()
			{
				CheckVersion();
				_itemIndex = -1;
				_reachedEnd = false;
				_itemEnumerator.Reset();
			}
			
			// Private Methods (1)

			private void CheckVersion()
			{
				if(_originalVersion != _multiset._version) {
					throw new InvalidOperationException("List has been changed.");
				}
			}

			#endregion Methods

			#region Properties (2)

			public T Current
			{
				get
				{
					CheckVersion();
					if(_itemIndex < 0 || _reachedEnd) {
						return default(T);
					} else {
						return _itemEnumerator.Current;
					}
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get { return Current; }
			}

			#endregion Properties

			#region Fields (5)

			private IEnumerator<T> _itemEnumerator;
			private int _itemIndex;
			private MultiSet<T> _multiset;
			private int _originalVersion;
			private bool _reachedEnd;

			#endregion Fields
		}
		#endregion Nested Classes

		#region Constructors (2)

		public MultiSet(IEnumerable<T> collection)
			: this()
		{
			foreach(T item in collection) {
				Add(item);
			}
		}

		public MultiSet()
		{
			_items = new Dictionary<T, int>();
			_version = 0;
		}

		#endregion Constructors

		#region Methods (5)

		// Public Methods (5)

		public override void Add(T item)
		{
			_version++;
			int itemCount;
			if(_items.TryGetValue(item, out itemCount)) {
				itemCount++;
				_items[item] = itemCount;
			} else {
				_items.Add(item, 1);
			}
			_count++;
		}

		public override void Clear()
		{
			_version++;
			_count = 0;
			_items.Clear();
		}

		public override bool Contains(T item)
		{
			return _items.ContainsKey(item);
		}

		public override IEnumerator<T> GetEnumerator()
		{
			return new MultiSetEnumerator(this);
		}

		public override bool Remove(T item)
		{
			_version++;
			bool success;
			int itemCount;
			if(_items.TryGetValue(item, out itemCount)) {
				Debug.Assert(itemCount > 0);
				itemCount--;
				if(itemCount == 0) {
					_items.Remove(item);
				} else {
					_items[item] = itemCount;
				}
				_count--;
				success = true;
			} else {
				success = false;
			}
			return success;
		}

		#endregion Methods

		#region Properties (1)

		public override int Count {
			get {
				return _count;
			}
		}

		#endregion Properties

		#region Fields (3)

		private int _count;
		private readonly Dictionary<T, int> _items;
		private int _version;

		#endregion Fields
	}
}
