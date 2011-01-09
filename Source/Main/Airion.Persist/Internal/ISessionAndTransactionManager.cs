// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using NHibernate;

namespace Airion.Persist.Internal
{
	public interface ISessionAndTransactionManager : IDisposable
	{
		ISession Session { get; }
	}
	
	public delegate ISessionAndTransactionManager CreateSessionAndTransactionManager(IPersistenceProvider provider);
}
