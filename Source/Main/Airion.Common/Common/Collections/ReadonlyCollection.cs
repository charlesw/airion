// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Collections;

namespace Airion.Common.Collections
{
	public abstract class ReadonlyCollectionBase<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
	{
		#region Constructors

		public ReadonlyCollectionBase()
		{
		}

		#endregion Constructors 

		#region Methods 

		// Public Methods

		/// <inheritdoc />
		public virtual bool Contains(T item)
		{			
			bool result = false;
			foreach(T current in this) {
				if(Object.Equals(current, item)) {
					result = true;
					break;
				}
			}
			return result;
		}

		/// <inheritdoc />
		public void CopyTo(T[] array, int arrayIndex)
		{
			Guard.RequireNotNull("array", array);
			Guard.RequirePositiveInteger("arrayIndex", arrayIndex);
			CollectionGuard.RequireArrayCanFitCollection("array", this, array, arrayIndex);
			
			foreach(T current in this) {
				array[arrayIndex] = current;
				arrayIndex++;
			}
		}

		/// <inheritdoc />
		public abstract IEnumerator<T> GetEnumerator();
		
		// Private Methods
		
		/// <inheritdoc />
		void ICollection<T>.Add(T item)
		{			
			CollectionGuard.ModifiedReadonlyCollection(GetType());
		}
		/// <inheritdoc />
		void ICollection<T>.Clear()
		{
			CollectionGuard.ModifiedReadonlyCollection(GetType());
		}
		
		/// <inheritdoc />
		void ICollection.CopyTo(Array array, int index)
		{
			CopyTo((T[])array, index);
		}		

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		/// <inheritdoc />
		bool ICollection<T>.Remove(T item)
		{
			CollectionGuard.ModifiedReadonlyCollection(GetType());
			
			return false;
		}

		#endregion Methods 

		#region Properties

		/// <inheritdoc />
		public abstract int Count {
			get;
		}

		/// <inheritdoc />
		bool ICollection.IsSynchronized {
			get {
				return false;
			}
		}

		/// <inheritdoc />
		object ICollection.SyncRoot {
			get {
				return _syncRoot;
			}
		}

		/// <inheritdoc />
		bool ICollection<T>.IsReadOnly {
			get { return true; }
		}

		#endregion Properties 

		#region Fields

		private object _syncRoot;

		#endregion Fields 
	}
}
