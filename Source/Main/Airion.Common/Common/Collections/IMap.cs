// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Collections;

namespace Airion.Common.Collections
{
	/// <summary>
	/// Description of IMap.
	/// </summary>
	public interface IMap<TKey, TItem> : ICollection<TItem>, IEnumerable<TItem>
		where TItem : IIdentifiable<TKey>
	{
		bool ContainsKey(TKey key);
		 
		bool TryGetItem(TKey key, out TItem item);
		
		TItem this[TKey key]
		{
			get; set;
		}
		
		bool Remove(TKey key);	
	}
}
