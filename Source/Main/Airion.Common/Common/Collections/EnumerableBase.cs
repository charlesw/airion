// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Collections;

namespace Airion.Common.Collections
{
	public abstract class EnumerableBase<T> : IEnumerable<T>, IEnumerable
	{
		#region Methods (2)

		// Public Methods (1)

		public abstract IEnumerator<T> GetEnumerator();
		// Private Methods (1)

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion Methods
	}
}
