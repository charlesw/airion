// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	/// <summary>
	/// Description of Map.
	/// </summary>
	public class Map<TKey, TItem> : CollectionBase<TItem> , IMap<TKey, TItem>, IFreezable
		where TItem : IIdentifiable<TKey>
	{
		#region Constructors

		public Map(IEnumerable<TItem> collection)
		{
			foreach(TItem item in collection) {
				Guard.Require("collection", item != null, "Collection must not contain null items.");
				items.Add(item.Key, item);
			}
		}

		public Map()
		{
		}

		#endregion Constructors

		#region Methods

		// Public Methods

		public override void Add(TItem item)
		{
			CheckFrozenState();
			items.Add(item.Key, item);
			OnItemAdded(item);
		}

		public override void Clear()
		{
			CheckFrozenState();
			OnClearing();
			items.Clear();
		}

		public override bool Contains(TItem item)
		{
			return items.ContainsKey(item.Key);
		}

		public bool ContainsKey(TKey key)
		{
			return items.ContainsKey(key);
		}

		public void Freeze()
		{
			isFrozen = true;
			OnFreeze();
		}

		public override IEnumerator<TItem> GetEnumerator()
		{
			return items.Values.GetEnumerator();
		}

		public override bool Remove(TItem item)
		{
			CheckFrozenState();
			bool success = false;
			if(items.Remove(item.Key)) {
				OnItemRemoved(item);
				success = true;
			}
			return success;
		}

		public bool Remove(TKey key)
		{
			CheckFrozenState();
			bool success;
			TItem item;
			if(items.TryGetValue(key, out item)) {
				success = Remove(item);
			} else {
				success = false;
			}
			return success;
		}

		public object Thaw()
		{
			Map<TKey, TItem> clone = (Map<TKey, TItem>)this.MemberwiseClone();
			clone.isFrozen = false;
			TransferMembers(clone);
			return clone;
		}

		public bool TryGetItem(TKey key, out TItem item)
		{
			return items.TryGetValue(key, out item);
		}

		// Protected Methods

		protected void CheckFrozenState()
		{
			Guard.Operation(!isFrozen, "Cannot modify a frozen object.");
		}

		protected virtual void OnClearing()
		{
			
		}

		protected virtual void OnFreeze()
		{
			
		}

		protected virtual void OnItemAdded(TItem item)
		{
			
		}

		protected virtual void OnItemRemoved(TItem item)
		{
			
		}

		protected virtual void TransferMembers(Map<TKey, TItem> clone)
		{
			clone.items = new Dictionary<TKey, TItem>();
			foreach(TItem item in items.Values) {
				if(item is IFreezable) {
					clone.items.Add(item.Key, (TItem)((IFreezable)item).Thaw());
				} else {
					clone.items.Add(item.Key, item);
				}
			}
		}


		#endregion Methods

		#region Properties

		public override int Count {
			get {
				return items.Count;
			}
		}

		public bool IsFrozen {
			get {
				return isFrozen;
			}
		}

		public TItem this[TKey key]
		{
			get { return items[key]; }
			set	{
				CheckFrozenState();
				TItem item;
				if(items.TryGetValue(key, out item)) {
					if(!Object.ReferenceEquals(item, value)) {
						items[key] = value;
						OnItemRemoved(item);
						OnItemAdded(item);
					}
				} else {
					items[key] = value;
				}
			}
		}

		#endregion Properties

		#region Fields

		private bool isFrozen = false;
		protected Dictionary<TKey, TItem> items = new Dictionary<TKey, TItem>();

		#endregion Fields
	}
}
