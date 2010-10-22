// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.TransientProvider
{
	/// <summary>
	/// Description of TransientPersistenceProvider.
	/// </summary>
	public class TransientPersistenceProvider : LightDisposableBase, IPersistenceProvider
	{
		public TransientPersistenceProvider()
		{
		}
		
		public ISession OpenSession()
		{
			var session = new TransientSession(this);
			OnSessionOpened(new SessionEventArgs(session));
			return session;
		}
		
		
		protected virtual void OnSessionOpened(SessionEventArgs args)
		{
			if(SessionOpened != null) {
				SessionOpened(this, args);
			}
		}
		
		public event EventHandler<SessionEventArgs> SessionOpened;
	}
}
