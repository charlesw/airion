// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	/// <summary>
	/// Notifies clients when a property value is changing.
	/// </summary>
	public interface INotifyPropertyChanging
	{
		/// <summary>
		/// Occurs when a property is changing.
		/// </summary>
		/// <remarks>
		/// This event should be fired just before the property is changed,
		/// but after any constraints that could prevent the change have
		/// occured (i.e. null checks etc).
		/// </remarks>
		event EventHandler<PropertyChangingEventArgs> PropertyChanging;
	}
}
