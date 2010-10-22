// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.Internal
{
	/// <summary>
	/// Description of SessionAndTransactionManager.
	/// </summary>
	public class SessionAndTransactionManager : LightDisposableBase
	{
		private IPersistenceProvider _provider;
		private ISession _session;
		private ITransaction _transaction;
		
		public SessionAndTransactionManager(IPersistenceProvider provider)
		{
			_provider = provider;
		}
		
		public ISession Session
		{
			get {
				if(_session == null) {
					_session = _provider.OpenSession();
					
					_transaction = _session.BeginTransaction();
				}
				return _session;
			}
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(_transaction != null) {
					_transaction.Commit();
					_transaction.Dispose();
					_transaction = null;
				}
				
				if(_session != null) {
					_session.Dispose();
					_session = null;
				}
			}
			base.Dispose(disposing);
		}		
	}
}
