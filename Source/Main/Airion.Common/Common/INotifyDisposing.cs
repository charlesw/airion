// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	/// <summary>
	/// Description of INotifyDisposing.
	/// </summary>
	public interface INotifyDisposing : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether the object is in the process of being disposed of.
		/// </summary>
		bool IsDisposing { get; }
		
		/// <summary>
		/// Occurs just before the object is disposed of by a call to the <see cref="IDisposable.Dispose()" /> method.
		/// </summary>
		event EventHandler<EventArgs> Disposing;
	}
}
