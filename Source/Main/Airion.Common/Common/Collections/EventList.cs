// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.ComponentModel;

namespace Airion.Common.Collections
{
	public sealed class EventList<T> : Airion.Common.Collections.ArrayListBase<T>
	{
		#region Nested Classes (1)
		
		public class ItemEventArgs : EventArgs
		{
			#region Constructors (1)

			public ItemEventArgs(T item, int index)
			{
				Item = item;
				Index = index;
			}

			#endregion Constructors

			#region Properties (2)

			public int Index { get; private set; }

			public T Item { get; private set; }

			#endregion Properties
		}
		
		#endregion Nested Classes

		#region Methods (6)

		// Public Methods (2)

		public override void Insert(int index, T item)
		{
			base.Insert(index, item);
			OnItemAdded(new ItemEventArgs(item, index));
		}

		public override void RemoveAt(int index)
		{
			T item = this[index];
			base.RemoveAt(index);
			OnItemRemoved(new ItemEventArgs(item, index));
		}
		// Private Methods (4)

		private void OnItemAdded(ItemEventArgs e)
		{
			if (e.Item is INotifyPropertyChanged) {
				((INotifyPropertyChanged)e.Item).PropertyChanged += OnItemPropertyChanged;
			}

			if (ItemAdded != null) {
				ItemAdded(this, e);
			}
		}

		private void OnItemChanged(ItemEventArgs e)
		{
			if (ItemChanged != null) {
				ItemChanged(this, e);
			}
		}

		private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			T item = (T)sender;
			OnItemChanged(new ItemEventArgs(item, IndexOf(item)));
		}

		private void OnItemRemoved(ItemEventArgs e)
		{
			if (e.Item is INotifyPropertyChanged) {
				((INotifyPropertyChanged)e.Item).PropertyChanged -= OnItemPropertyChanged;
			}

			if (ItemRemoved != null) {
				ItemRemoved(this, e);
			}
		}

		#endregion Methods

		#region Properties (1)

		public override T this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				T item = base[index];
				OnItemRemoved(new ItemEventArgs(item, index));
				base[index] = value;
				OnItemAdded(new ItemEventArgs(value, index));
			}
		}

		#endregion Properties

		#region Delegates and Events (3)

		// Events (3)

		public event EventHandler<ItemEventArgs> ItemAdded;

		public event EventHandler<ItemEventArgs> ItemChanged;

		public event EventHandler<ItemEventArgs> ItemRemoved;

		#endregion Delegates and Events
	}
}
