// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	/// <summary>
	/// Description of INotifyDisposed.
	/// </summary>
	public interface INotifyDisposed : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether the object has been disposed of.
		/// </summary>
		bool IsDisposed {get;}
		
		/// <summary>
		/// Occurs after the object is disposed of by a call to the <see cref="IDisposable.Dispose()" /> method.
		/// </summary>
		event EventHandler<EventArgs> Disposed;
	}
}
