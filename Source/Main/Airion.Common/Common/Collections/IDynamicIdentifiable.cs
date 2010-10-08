// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common.Collections
{
	/// <summary>
	/// Description of IDynamicIdentifiable.
	/// </summary>
	public interface IDynamicIdentifiable<TKey> : IIdentifiable<TKey>
	{
		event EventHandler<ValueChangedEventArgs<TKey>> KeyChanged;		
	}
}
