// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public class DynamicMap<TKey, TItem> : Map<TKey, TItem>
		where TItem : IDynamicIdentifiable<TKey>
	{
		#region Constructors 

		public DynamicMap()
		{
		}

		public DynamicMap(IEnumerable<TItem> map)
			: base(map)
		{
		}
		
		#endregion Constructors 

		#region Methods 

		protected override void OnItemAdded(TItem item)
		{
			item.KeyChanged += OnKeyChanged;
		}
		
		protected override void OnItemRemoved(TItem item)
		{
			item.KeyChanged -= OnKeyChanged;
		}
		
		protected override void OnClearing()
		{
			foreach(TItem item in this) {
				OnItemRemoved(item);
			}
		}
		
		// Private Methods 

		private void OnKeyChanged(object sender, ValueChangedEventArgs<TKey> e)
		{
			TItem item = (TItem)sender;
			items.Remove(e.OldValue);
			items.Add(e.NewValue, item);
		}


		#endregion Methods 
	}
}

