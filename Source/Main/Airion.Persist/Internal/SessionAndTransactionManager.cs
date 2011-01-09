// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using NHibernate;

namespace Airion.Persist.Internal
{
	/// <summary>
	/// Description of SessionAndTransactionManager.
	/// </summary>
	public class SessionAndTransactionManager : LightDisposableBase, ISessionAndTransactionManager
	{
		private ISession _session;
		private ITransaction _transaction;

		public SessionAndTransactionManager(IPersistenceProvider provider)
		{
			Provider = provider;
		}

		public ISession Session {
			get {
				if (_session == null) {
					var session = Provider.OpenSession();
					OnOpenSession(session);
					_session = session;
					
					_transaction = _session.BeginTransaction();
					OnBeginTransaction(_transaction);
				}
				return _session;
			}
		}
		
		protected virtual void OnOpenSession(ISession session)
		{
			
		}
		
		protected virtual void OnBeginTransaction(ITransaction transaction)
		{
			
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (_transaction != null) {
					_transaction.Commit();
					_transaction.Dispose();
					_transaction = null;
				}

				if (_session != null) {
					_session.Dispose();
					_session = null;
				}
			}
			base.Dispose(disposing);
		}
		
		protected IPersistenceProvider Provider { get; private set; }
	}
}
