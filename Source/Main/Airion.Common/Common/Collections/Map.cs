// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Airion.Common.Collections
{
	public class Map<TKey, TItem> : CollectionBase<TItem> , IMap<TKey, TItem>, IFreezable
	{
		#region Fields

		private bool isFrozen = false;
		protected Dictionary<TKey, TItem> items = new Dictionary<TKey, TItem>();
		private Func<TItem, TKey> _getKeyFromItem;
		private string _itemKeyName;
		
		#endregion Fields
		
		#region Constructors

		public Map(Expression<Func<TItem, TKey>> itemKeyPropertySelector, IEnumerable<TItem> collection)
			: this(itemKeyPropertySelector)
		{
			foreach(TItem item in collection) {
				Guard.Require("collection", item != null, "Collection must not contain null items.");
				items.Add(GetItemKey(item), item);
			}
		}

		public Map(Expression<Func<TItem, TKey>> itemKeyPropertySelector)
		{
			_getKeyFromItem = itemKeyPropertySelector.Compile();
			_itemKeyName = ReflectionUtilities.GetPropertyName(itemKeyPropertySelector);
		}

		#endregion Constructors

		#region Methods

		// Public Methods

		public override void Add(TItem item)
		{
			CheckFrozenState();
			items.Add(GetItemKey(item), item);
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
			return items.ContainsKey(GetItemKey(item));
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
			if(items.Remove(GetItemKey(item))) {
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

		protected virtual void OnFreeze()
		{
			
		}

		protected virtual void OnClearing()
		{
			foreach(var item in items.Values) {
				var notifiableItem = item as INotifyPropertyChanging;
				if(notifiableItem != null) {
					notifiableItem.PropertyChanging -= ItemPropertyChanging;
				}
			}
		}

		protected virtual void OnItemAdded(TItem item)
		{
			var notifiableItem = item as INotifyPropertyChanging;
			if(notifiableItem != null) {
				notifiableItem.PropertyChanging += ItemPropertyChanging;
			}
		}
		protected virtual void OnItemRemoved(TItem item)
		{
			var notifiableItem = item as INotifyPropertyChanging;
			if(notifiableItem != null) {
				notifiableItem.PropertyChanging -= ItemPropertyChanging;
			}
		}

		protected virtual void TransferMembers(Map<TKey, TItem> clone)
		{
			clone.items = new Dictionary<TKey, TItem>();
			foreach(TItem item in items.Values) {
				if(item is IFreezable) {
					clone.items.Add(GetItemKey(item), (TItem)((IFreezable)item).Thaw());
				} else {
					clone.items.Add(GetItemKey(item), item);
				}
			}
		}

		protected TKey GetItemKey(TItem item)
		{
			return _getKeyFromItem(item);
		}


		private void ItemPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if(e.PropertyName == _itemKeyName) {
				CheckFrozenState();
				TKey newValue = (TKey)e.NewValue;
				if(items.ContainsKey(newValue)) {
					throw new ArgumentException(String.Format("Another item with the key '{0}' already exists.", newValue), e.PropertyName);
				} else {
					TKey oldValue = (TKey)e.OldValue;
					items.Remove(oldValue);
					items.Add(newValue, (TItem)sender);
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
		}

		#endregion Properties
	}
}
