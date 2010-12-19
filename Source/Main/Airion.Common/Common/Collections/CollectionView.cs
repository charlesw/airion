// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Airion.Common.Collections
{
	/// <summary>
	/// Represents a synchronizing view into another collection.
	/// </summary>
	/// <remarks>
	/// A <see cref="CollectionView<,>" /> is used to key items sychronized in the case of 
	/// a Bi-directional relationship between the collections owner and the item.
	/// </remarks>
	public abstract class CollectionView<TOwner, TItem> : CollectionBase<TItem>
	{
		private int _updateCount;
		public TOwner Owner { get; private set; }
		private ICollection<TItem> _backingCollection;
		
		public CollectionView(TOwner owner, ICollection<TItem> backingCollection)
		{
			_updateCount = 0;
			Owner = owner;
			_backingCollection = backingCollection;
		}
			
		public void BeginUpdate()
		{			
			_updateCount++;
		}
		
		public void EndUpdate()
		{
			Guard.Operation(_updateCount > 0, "EndUpdate must have a corresponding BeginUpdate.");
			_updateCount--;
		}
		
		/// <summary>
		/// Gets a boolean value reflecting if the synchronizing collection is currently in a update block.
		/// </summary>
		public bool IsUpdating { 
			get { return _updateCount > 0; }
		}
		
		public override bool Remove(TItem item)
		{
			bool success = false;
			if(!IsUpdating && _backingCollection.Contains(item)) {
				try {
					BeginUpdate();
					success = _backingCollection.Remove(item);
					RemoveOwnerFromItem(item, Owner);
				} finally {
					EndUpdate();
				}
			}
			return success;
		}
		
		public override IEnumerator<TItem> GetEnumerator()
		{
			return _backingCollection.GetEnumerator();
		}
		
		public override int Count {
			get {
				return _backingCollection.Count;
			}
		}
		
		public override void Clear()
		{
			if(!IsUpdating) {
				try {
					BeginUpdate();
					var items = _backingCollection.ToList();
					_backingCollection.Clear();
					foreach(var item in items) {
						RemoveOwnerFromItem(item, Owner);
					}
				} finally {
					EndUpdate();
				}
			}
		}
		
		public override void Add(TItem item)
		{
			if(!IsUpdating && !_backingCollection.Contains(item)) {
				try {
					BeginUpdate();
					_backingCollection.Add(item);
					AddOwnerToItem(item, Owner);
				} finally {
					EndUpdate();
				}
			}
		}
		
		protected abstract void AddOwnerToItem(TItem item, TOwner owner);
		
		protected abstract void RemoveOwnerFromItem(TItem item, TOwner owner);
	}
}
