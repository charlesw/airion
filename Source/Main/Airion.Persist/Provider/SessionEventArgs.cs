// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.Provider
{
	/// <summary>
	/// Description of SessionEventArgs.
	/// </summary>
	public class SessionEventArgs : EventArgs
	{
		public SessionEventArgs(ISession session)
		{
			Session = session;
		}
		
		public ISession Session { get; private set; }
	}
}
