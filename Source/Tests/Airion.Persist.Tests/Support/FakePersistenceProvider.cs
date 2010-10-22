// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of FakePersistenceProvider.
	/// </summary>
	public class FakePersistenceProvider : LightDisposableBase, IPersistenceProvider
	{
		public FakePersistenceProvider()
		{
		}
		
		public ISession OpenSession()
		{
			return new FakeSession(this);
		}
		
		public event EventHandler<SessionEventArgs> SessionOpened;
	}
}
