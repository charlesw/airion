// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using INHTransaction = NHibernate.ITransaction;

namespace Airion.Persist.NHibernateProvider
{
	/// <summary>
	/// Description of NHibernateTransaction.
	/// </summary>
	public class NHibernateTransaction : LightDisposableBase, ITransaction
	{
		private INHTransaction _transaction;
		
		public NHibernateTransaction(INHTransaction transaction)
		{
			_transaction = transaction;
		}
		
		public void Commit()
		{
			CheckState();
			_transaction.Commit();
		}
		
		public void Rollback()
		{
			CheckState();
			_transaction.Rollback();
		}
		
		public bool IsActive
		{
			get {
				CheckState();
				return _transaction.IsActive;
			}
		}
		
		public bool WasCommitted
		{
			get {
				CheckState();
				return _transaction.WasCommitted;
			}
		}
		
		public bool WasRolledBack
		{
			get {
				CheckState();
				return _transaction.WasRolledBack;
			}
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(_transaction != null) {
					_transaction.Dispose();
					_transaction = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}
