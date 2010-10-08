// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)
                
using System;
using System.Collections;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	/// <summary>
	/// Description of ListCollectionWrapper.
	/// </summary>
	public class ListCollectionWrapper : ListCollectionWrapperBase
	{
		private List<IList> lists;
		
		public ListCollectionWrapper(IEnumerable<IList> lists)
		{
			this.lists = new List<IList>(lists);
		}
		
		protected override IEnumerable<IList> GetWrappedLists()
		{
			return lists;
		}
	}
}
