// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public abstract class CollectionBase<T> : EnumerableBase<T>, ICollection<T>, ICollection
	{
		#region Constructors (1)

		public CollectionBase()
		{
		}

		#endregion Constructors

		#region Methods (6)

		// Public Methods (5)

		public abstract void Add(T item);

		public abstract void Clear();

		public virtual bool Contains(T item)
		{
			var enumerator = GetEnumerator();
			while(enumerator.MoveNext()) {
				if(Object.Equals(enumerator.Current, item)) {
					return true;
				}
			}
			return false;
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			Guard.RequireNotNull("array", array);
			Guard.RequirePositiveInteger("arrayIndex", arrayIndex);
			CollectionGuard.RequireArrayCanFitCollection("array", this, array, arrayIndex);
			
			int index = arrayIndex;
			var enumerator = GetEnumerator();
			while(enumerator.MoveNext()) {
				array[index] = enumerator.Current;
				index++;
			}
		}

		public abstract bool Remove(T item);
		
		// Private Methods (1)

		void ICollection.CopyTo(Array array, int index)
		{
			CopyTo((T[])array, index);
		}

		#endregion Methods

		#region Properties (4)

		public abstract int Count { get; }

		bool ICollection.IsSynchronized {
			get {
				return false;
			}
		}

		object ICollection.SyncRoot {
			get {
				return _syncRoot;
			}
		}

		bool ICollection<T>.IsReadOnly {
			get {
				return false;
			}
		}

		#endregion Properties

		#region Fields (1)

		private readonly object _syncRoot = new Object();

		#endregion Fields
	}
}
