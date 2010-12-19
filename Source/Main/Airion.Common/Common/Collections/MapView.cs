// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Airion.Common.Collections
{
	public abstract class MapView<TOwner, TKey, TItem> : CollectionBase<TItem> , IMap<TKey, TItem>
	{
		#region Fields

		protected IDictionary<TKey, TItem> _backingCollection;
		private int _reentrancyBlock;
		
		#endregion Fields
		
		#region Constructors

		public MapView(TOwner owner, IDictionary<TKey, TItem> collection)
		{
			Owner = owner;
			_backingCollection = collection;
		}

		#endregion Constructors

		#region Methods

		// Public Methods

		public override void Add(TItem item)
		{
			BeginUpdate();
			try {
				_backingCollection.Add(GetItemKey(item), item);
				AddOwnerToItem(item, Owner);
			} finally {
				EndUpdate();
			}
		}

		public override void Clear()
		{
			try {
				BeginUpdate();
				var items = _backingCollection.Values.ToList();
				_backingCollection.Clear();
				foreach(var item in items) {
					RemoveOwnerFromItem(item, Owner);
				}
			} finally {
				EndUpdate();
			}
		}

		public override bool Contains(TItem item)
		{
			return _backingCollection.ContainsKey(GetItemKey(item));
		}

		public bool ContainsKey(TKey key)
		{
			return _backingCollection.ContainsKey(key);
		}

		public override IEnumerator<TItem> GetEnumerator()
		{
			return _backingCollection.Values.GetEnumerator();
		}

		public override bool Remove(TItem item)
		{
			try {
				BeginUpdate();
				bool success = false;
				if(_backingCollection.Remove(GetItemKey(item))) {
					RemoveOwnerFromItem(item, Owner);
					success = true;
				}
				return success;
			} finally {
				EndUpdate();
			}
		}

		public bool Remove(TKey key)
		{
			try {
				BeginUpdate();
				bool success;
				TItem item;
				if(_backingCollection.TryGetValue(key, out item)) {
					success = Remove(item);
				} else {
					success = false;
				}
				return success;
			} finally {
				EndUpdate();
			}
		}
		
		public bool TryGetItem(TKey key, out TItem item)
		{
			return _backingCollection.TryGetValue(key, out item);
		}
		
		/// <summary>
		/// Refreshes the item.
		/// </summary>
		/// <remarks>
		/// This will effectively remove the item with the specified key
		/// from the backing collection and add the new item.
		/// </remarks>
		/// <param name="key">The key of the item to be removed.</param>
		/// <param name="item">The item to be added.</param>
		public void Refresh(TKey oldKey, TItem item)
		{
			var newKey = GetItemKey(item);
			Guard.Require("oldKey", _backingCollection.ContainsKey(oldKey), "No item exists in the backing collection with the specified oldKey '{0}'.", oldKey);
			Guard.Require("item.key", !_backingCollection.ContainsKey(newKey), "Another item with the key '{0}' already exists.", newKey);
			Guard.RequireEqual("item", item, _backingCollection[oldKey]);
			
			try {
				BeginUpdate();			
				if(!Object.Equals(oldKey, newKey)) {
					_backingCollection.Remove(oldKey);
					_backingCollection.Add(newKey, item);
				}
			} finally {
				EndUpdate();
			}
		}

		// Protected Methods
		
		protected abstract TKey GetItemKey(TItem item);

		protected abstract void AddOwnerToItem(TItem item, TOwner owner);
		
		protected abstract void RemoveOwnerFromItem(TItem item, TOwner owner);
		
		private void BeginUpdate()
		{
			_reentrancyBlock++;
		}
		
		private void EndUpdate()
		{
			Guard.Operation(_reentrancyBlock>0, "EndUpdate must have a corresponding BeginUpdate.");
			_reentrancyBlock--;
		}
		
		public bool InUpdateBlock
		{
			get { return _reentrancyBlock > 0; }
		}
		
		#endregion Methods

		#region Properties

		public TOwner Owner { get; private set; }
		
		public override int Count {
			get {
				return _backingCollection.Count;
			}
		}

		public TItem this[TKey key]
		{
			get { return _backingCollection[key]; }
		}

		#endregion Properties
	}
}
