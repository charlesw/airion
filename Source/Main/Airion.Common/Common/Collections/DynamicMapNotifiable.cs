// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public class DynamicNotifiableMap<TKey, TItem> : DynamicMap<TKey, TItem>
		where TItem : IDynamicIdentifiable<TKey>
	{
		public DynamicNotifiableMap()
		{
		}
		
		public DynamicNotifiableMap(IEnumerable<TItem> map)
			: base(map)
		{
		}
		
		protected override void OnClearing()
		{
			base.OnClearing();
			OnListChanged(new ListClearingEventArgs());
		}
		
		protected override void OnItemRemoved(TItem item)
		{
			base.OnItemRemoved(item);
			OnListChanged(new ListItemRemovedEventArgs(item));
		}
		
		protected override void OnItemAdded(TItem item)
		{
			base.OnItemAdded(item);
			OnListChanged(new ListItemAddedEventArgs(item));
		}
		
		public event EventHandler<ListChangedEventArgs> ListChanged;
		
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if(ListChanged != null) {
				ListChanged(this, e);
			}
		}
		
		public abstract class ListChangedEventArgs : EventArgs
		{
			public ListChangedEventArgs() 
			{				
			}
		}
		
		public sealed class ListItemAddedEventArgs : ListChangedEventArgs
		{
			public ListItemAddedEventArgs(TItem item)
				: base()
			{
				Item = item;
			}
			
			public TItem Item { get; private set; }
		}
		
		public sealed class ListItemRemovedEventArgs : ListChangedEventArgs
		{
			public ListItemRemovedEventArgs(TItem item)
				: base()
			{
				Item = item;
			}
			
			public TItem Item { get; private set; }
		}
		
		public sealed class ListClearingEventArgs : ListChangedEventArgs
		{
			public ListClearingEventArgs()
				: base()
			{
			}		
		}
	}
}
