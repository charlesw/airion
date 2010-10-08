// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public abstract class ListBase<T> : CollectionBase<T>, IList<T>
	{
		#region Nested Classes (1)

		private class ListEnumerator : IEnumerator<T>
		{
			#region Constructors (1)

			public ListEnumerator(ListBase<T> list)
			{
				_list = list;
				_originalVersion = list.Version;
				_index = -1;
			}

			#endregion Constructors

			#region Methods (4)

			// Public Methods (3)

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				CheckVersion();
				_index++;
				return _index < _list.Count;
			}

			public void Reset()
			{
				_index = -1;
			}
			// Private Methods (1)

			private void CheckVersion()
			{
				if(_originalVersion != _list.Version) {
					throw new InvalidOperationException("List has been changed.");
				}
			}

			#endregion Methods

			#region Properties (2)

			public T Current {
				get {
					CheckVersion();
					if(_index < 0 || _index >= _list.Count) {
						return default(T);
					} else {
						return _list[_index];
					}
				}
			}

			object System.Collections.IEnumerator.Current {
				get {
					return Current;
				}
			}

			#endregion Properties

			#region Fields (3)

			private int _index;
			private ListBase<T> _list;
			private int _originalVersion;

			#endregion Fields
		}
		
		#endregion Nested Classes

		#region Constructors (1)

		public ListBase()
		{
		}

		#endregion Constructors

		#region Methods (7)

		// Public Methods (6)

		public override void Add(T item)
		{
			Insert(Count, item);
		}

		public override IEnumerator<T> GetEnumerator()
		{
			return new ListEnumerator(this);
		}

		public virtual int IndexOf(T item)
		{
			for (int i = 0; i < Count; i++) {
				if(Object.Equals(this[i], item)) {
					return i;
				}
			}
			return -1;
		}

		public abstract void Insert(int index, T item);

		public override bool Remove(T item)
		{
			int index = IndexOf(item);
			bool success;
			if(index >= 0) {
				RemoveAt(index);
				success = true;
			} else {
				success = false;
			}
			return success;
		}

		public abstract void RemoveAt(int index);
		// Protected Methods (1)

		protected void UpdateVersion()
		{
			Version++;
		}

		#endregion Methods

		#region Properties (2)

		public abstract T this[int index] {
			get;
			set;
		}

		protected int Version { get; private set; }

		#endregion Properties
	}
}
