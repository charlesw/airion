// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	/// <summary>
	/// Description of IValueStore.
	/// </summary>
	public interface IValueStore<T>
	{
		T Value { get; set; }
	}
}
