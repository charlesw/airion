// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common.Collections
{
	/// <summary>
	/// Description of IIdentifiable.
	/// </summary>
	public interface IIdentifiable<TKey>
	{
		TKey Key { get; }
	}
}
