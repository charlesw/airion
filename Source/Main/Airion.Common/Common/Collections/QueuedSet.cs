// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public sealed class QueuedSet<T> : LinkedList<T>, ICloneable
	{
		#region Constructors

		public QueuedSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: base()
		{
			Comparer = comparer;
			foreach(T item in collection) {
				Enqueue(item);
			}
		}

		public QueuedSet(IEqualityComparer<T> comparer)
			: base()
		{
			Comparer = comparer;
		}

		public QueuedSet(IEnumerable<T> collection)
			: this(collection, EqualityComparer<T>.Default)
		{
			
		}

		private QueuedSet(QueuedSet<T> other)
		{
			Comparer = other.Comparer;
			foreach(T item in other) {
				AddLast(item);
			}
		}

		public QueuedSet()
			: this(EqualityComparer<T>.Default)
		{
		}

		#endregion Constructors

		#region Methods

		// Public Methods

		public object Clone()
		{
			return new QueuedSet<T>(this);
		}

		public T Dequeue()
		{
			T result = First.Value;
			this.RemoveFirst();
			return result;
		}

		public void Enqueue(T item)
		{
			LinkedListNode<T> node = Last;
			while(node != null && !Comparer.Equals(node.Value, item)) {
				node = node.Previous;
			}
			
			if(node == null) {
				this.AddLast(item);
			} else if(node != Last) {
				this.Remove(node);
				this.AddLast(item);
			}
		}


		#endregion Methods

		#region Properties

		public IEqualityComparer<T> Comparer
		{
			get;
			private set;
		}

		#endregion Properties
	}
}
